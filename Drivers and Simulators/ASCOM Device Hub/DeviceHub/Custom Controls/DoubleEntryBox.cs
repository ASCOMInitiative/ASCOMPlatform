using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ASCOM.DeviceHub
{
	/// <summary>
	/// DoubleEntryBox allows the user to type in valid real numbers only
	/// Verification is made by using the Double.Parse method.
	/// </summary>
	public class DoubleEntryBox : TextBox
    {
		#region Constructors

		public DoubleEntryBox()
		{
			DataObject.AddPastingHandler( this, OnPaste );
		}

		#endregion Constructors

		#region DependencyProperties

		public double Minimum
		{
			get { return (double)GetValue( MinimumProperty ); }
			set { SetValue( MinimumProperty, value ); }
		}

		public static readonly DependencyProperty MinimumProperty =
			DependencyProperty.Register( "Minimum", typeof( double ), typeof( DoubleEntryBox ), new PropertyMetadata( Double.MinValue ) );

		public double Maximum
		{
			get { return (double)GetValue( MaximumProperty ); }
			set { SetValue( MaximumProperty, value ); }
		}

		public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register( "Maximum", typeof( double ), typeof( DoubleEntryBox ), new PropertyMetadata( Double.MaxValue ) );

		public bool IsMaximumInclusive
		{
			get { return (bool)GetValue( IsMaximumInclusiveProperty ); }
			set { SetValue( IsMaximumInclusiveProperty, value ); }
		}

		public static readonly DependencyProperty IsMaximumInclusiveProperty =
			DependencyProperty.Register( "IsMaximumInclusive", typeof( bool ), typeof( DoubleEntryBox ), new PropertyMetadata( false ) );

		#endregion Dependency Properties

		#region Overrides

		protected override void OnPreviewKeyDown( KeyEventArgs e )
		{
			if ( e.Key == Key.Space )
			{
				e.Handled = true;
			}
		}

		protected override void OnPreviewTextInput( TextCompositionEventArgs e )
		{
			// Add the entered text to the current contents, at the caret position
			// and validate the result.

			string originalText = this.Text;
			string newText = e.Text;

			string text = MergeNewText( originalText, newText );

			if ( !IsValid( text ) )
			{
				e.Handled = true;
			}
		}

		#endregion Overrides

		#region Helper Methods

		private void OnPaste( object sender, DataObjectPastingEventArgs e )
		{
			bool isText = e.SourceDataObject.GetDataPresent( DataFormats.UnicodeText, true );

			if ( isText )
			{
				string originalText = this.Text;
				string newText = e.SourceDataObject.GetData( DataFormats.UnicodeText ) as String;

				int ndx = this.CaretIndex;
				string text = MergeNewText( originalText, newText );

				if ( IsValid( text ) )
				{
					this.Text = text;
					this.CaretIndex = ndx + newText.Length;
				}

				e.CancelCommand();
			}
		}

		private string MergeNewText( string originalText, string newText )
		{
			string selectedText = this.SelectedText;
			int ndx = this.CaretIndex;

			string text = originalText;

			if ( ndx <= text.Length )
			{
				if ( selectedText.Length > 0 )
				{
					text = text.Remove( ndx, selectedText.Length );
				}
				else if ( ndx < selectedText.Length )
				{
					text = text.Remove( ndx, newText.Length );
				}

				text = text.Insert( ndx, newText );
			}

			return text;
		}

		private bool IsValid( string text )
		{
			if ( text == "+" || text == "-" )
			{
				return true;
			}

			double dblValue;

			bool retval = Double.TryParse( text, out dblValue );

			if ( retval )
			{
				retval = IsInRange( dblValue );
			}

			return retval;
		}

		private bool IsInRange( double dblValue )
		{
			if ( dblValue < Minimum )
			{
				return false;
			}

			if ( dblValue > Maximum || ( dblValue == Maximum && !IsMaximumInclusive ) )
			{
				return false;
			}

			return true;
		}

		#endregion
	}
}