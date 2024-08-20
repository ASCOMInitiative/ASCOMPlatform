using System;
// Implements the filestore mechanic to store ASCOM profiles

// This implementation stores files in the All Users profile area that is accessible to everyone and so 
// creates a "per machine" store rather than a "per user" store.

using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using ASCOM.Utilities.Interfaces;
using Microsoft.VisualBasic;

namespace ASCOM.Utilities
{


    internal class AllUsersFileSystemProvider : IFileStoreProvider
    {
        private const string ASCOM_DIRECTORY = @"\ASCOM";
        private const string PROFILE_DIRECTORY = ASCOM_DIRECTORY + @"\Profile"; // Root directory within the supplied file system space

        private string BaseFolder, ASCOMFolder;

        #region New
        internal AllUsersFileSystemProvider()
        {
            // Find the location of the All Users profile
            ASCOMFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + ASCOM_DIRECTORY;
            BaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + PROFILE_DIRECTORY;
        }
        #endregion

        #region IFileStore Implementation
        internal void SetSecurityACLs(TraceLogger p_tl)
        {
            var dInfo = new DirectoryInfo(ASCOMFolder); // Apply to the ASCOM folder itself
            DirectorySecurity dSecurity;

            // PWGS 5.5.2.0 Fix for users security group not being globally usable
            // Build a temp domainSID using the Null SID passed in as a SDDL string. The constructor will 
            // accept the traditional notation or the SDDL notation interchangeably.
            var DomainSid = new SecurityIdentifier("S-1-0-0");
            // Create a security Identifier for the BuiltinUsers Group to be passed to the new accessrule
            var Ident = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, DomainSid);

            p_tl.LogMessage("  SetSecurityACLs", "Retrieving access control");
            dSecurity = dInfo.GetAccessControl();

            p_tl.LogMessage("  SetSecurityACLs", "Adding full control access rule");
            dSecurity.AddAccessRule(new FileSystemAccessRule(Ident, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

            p_tl.LogMessage("  SetSecurityACLs", "Setting access control");
            dInfo.SetAccessControl(dSecurity);

            p_tl.LogMessage("  SetSecurityACLs", "Successfully set security ACL!");
        }

        void IFileStoreProvider.SetSecurityACLs(TraceLogger p_tl) => SetSecurityACLs(p_tl);

        // Tests whether a file exists and returns a boolean value
        internal bool get_Exists(string p_FileName)
        {
            try
            {
                if (File.Exists(CreatePath(p_FileName)))
                {
                    return true; // This was successful 
                }
                else
                {
                    return false;
                } // No exception but no file returned so the file does not exist
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exists " + ex.ToString());
                return false; // Exception so file doesn't exist
            }
        }

        bool IFileStoreProvider.get_Exists(string p_FileName) => get_Exists(p_FileName);

        internal void CreateDirectory(string p_SubKeyName, TraceLogger p_TL)
        {
            // Creates a directory in the supplied path (p_SubKeyName)
            try
            {
                p_TL.LogMessage("  CreateDirectory", "Creating directory for: \"" + p_SubKeyName + "\"");
                Directory.CreateDirectory(CreatePath(p_SubKeyName));
                p_TL.LogMessage("  CreateDirectory", "Created directory OK");
            }
            catch (Exception ex)
            {
                p_TL.LogMessage("FileSystem.CreateDirectory", "Exception: " + ex.ToString());
                MessageBox.Show("CreateDirectory Exception: " + ex.ToString());
            }
        }

        void IFileStoreProvider.CreateDirectory(string p_SubKeyName, TraceLogger p_TL) => CreateDirectory(p_SubKeyName, p_TL);

        internal void DeleteDirectory(string p_SubKeyName)
        {
            // Deletes a directory specified by p_SubKeyName
            Directory.Delete(CreatePath(p_SubKeyName), true);
        }

        void IFileStoreProvider.DeleteDirectory(string p_SubKeyName) => DeleteDirectory(p_SubKeyName);

        internal void EraseFileStore()
        {
            MsgBoxResult Response;
            Response = MessageBox.Show("Are you sure you wish to erase the Utilities profile store?", MsgBoxStyle.OkCancel | MsgBoxStyle.Critical, "ASCOM.Utilities");
            if (Response == MsgBoxResult.Ok)
            {
                try
                {
                    Directory.Delete(BaseFolder, true);
                }
                catch
                {
                }
            }
        }

        void IFileStoreProvider.EraseFileStore() => EraseFileStore();

        // Enumerates the sub directories within a directory specified by p_SubKeyName
        // Returns a string array of directory names
        internal string[] get_GetDirectoryNames(string p_SubKeyName)
        {
            string[] FullDirs;
            int ct;
            string[] RelDirs;
            FullDirs = Directory.GetDirectories(CreatePath(p_SubKeyName));
            ct = 0;
            foreach (string FullDir in FullDirs)
            {
                RelDirs = Strings.Split(FullDir, @"\");
                FullDirs[ct] = RelDirs[RelDirs.Length - 1];
                ct += 1;
            }
            return FullDirs;
        }

        string[] IFileStoreProvider.get_GetDirectoryNames(string p_SubKeyName) => get_GetDirectoryNames(p_SubKeyName);

        internal string get_FullPath(string p_FileName)
        {
            return CreatePath(p_FileName);
        }

        string IFileStoreProvider.get_FullPath(string p_FileName) => get_FullPath(p_FileName);

        internal string BasePath
        {
            get
            {
                return BaseFolder;
            }
        }

        string IFileStoreProvider.BasePath { get => BasePath; }

        internal void Rename(string p_CurrentName, string p_NewName)
        {
            File.Delete(CreatePath(p_NewName)); // Make sure the target file doesn't exist
            File.Move(CreatePath(p_CurrentName), CreatePath(p_NewName));
        }

        void IFileStoreProvider.Rename(string p_CurrentName, string p_NewName) => Rename(p_CurrentName, p_NewName);

        internal void RenameDirectory(string CurrentName, string NewName)
        {
            try
            {
                Directory.Delete(CreatePath(NewName), true);
            }
            catch
            {
            } // Remove driectory if it already exists
            var DirInfo = new DirectoryInfo(CreatePath(CurrentName));
            DirInfo.MoveTo(CreatePath(NewName));
        }

        void IFileStoreProvider.RenameDirectory(string CurrentName, string NewName) => RenameDirectory(CurrentName, NewName);

        #endregion

        #region Support Code
        private string CreatePath(string p_FileName)
        {
            if (Strings.Left(p_FileName, 1) != @"\")
                p_FileName = @"\" + p_FileName;
            return BaseFolder + p_FileName;
        }
        #endregion

    }
}