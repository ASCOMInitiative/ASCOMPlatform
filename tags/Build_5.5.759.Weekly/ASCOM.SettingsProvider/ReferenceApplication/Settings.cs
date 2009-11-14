﻿using TiGra;

namespace ASCOM.SettingsProvider.ReferenceApplication.Properties
	{
    
    
    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    internal sealed partial class Settings {

		public Settings()
			{
			this.SettingChanging += this.SettingChangingEventHandler;
			this.SettingsSaving += this.SettingsSavingEventHandler;
			}

		private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e)
			{
			Diagnostics.TraceVerbose("SettingChanging event. Setting={0}, New value={1}", e.SettingName, e.NewValue);
			}

		private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
			{
			Diagnostics.TraceInfo("SettingsSaving event");
			}
    }
}
