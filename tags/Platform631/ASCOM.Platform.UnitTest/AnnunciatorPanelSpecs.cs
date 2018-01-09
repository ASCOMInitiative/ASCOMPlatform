using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ASCOM.Controls;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace ASCOM.Platform.UnitTest
{
    [Subject(typeof(AnnunciatorPanel ))]
    public class when_constructed : with_new_annunciator_panel
    {
        It should_have_correct_back_colour = () => Panel.BackColor.ShouldEqual(Color.FromArgb(64, 0, 0));
    }

    //[Subject(typeof (AnnunciatorPanel))]
    //public class When_panel_is_disposed : with_new_annunciator_panel_with_mock_annunciator
    //{
    //    Establish context = () => Annunciator.Setup(m=>m.Dispose());
    //    Because of = () => Panel.Dispose();
    //    It should_dispose_child_controls = () => Annunciator.Verify(m=>m.Dispose(), Times.AtLeastOnce());
    //}

    [Subject(typeof (AnnunciatorPanel))]
    public class When_panel_is_double_disposed : with_new_annunciator_panel
    {
        Because of = () => Ex = Catch.Exception(() =>
            {
                Panel.Dispose();
                Panel.Dispose();
            });
        It should_not_throw = () => Ex.ShouldBeNull();
    }

#region Contexts
    /// <summary>
    /// Constructs a new empty AnnunciatorPanel
    /// </summary>
    public class with_new_annunciator_panel
    {
        protected static AnnunciatorPanel Panel;
        Establish context = () => Panel = new AnnunciatorPanel();
        protected static Exception Ex;
    }

    /// <summary>
    /// Constructs an Annunciator Panel containing a single mock Annunciator control
    /// </summary>
    public class with_new_annunciator_panel_with_mock_annunciator
    {
        protected static AnnunciatorPanel Panel;
         protected static Exception Ex;
        protected static Mock<AnnunciatorPanel> Annunciator;

        Establish context = () =>
            {
                Panel = new AnnunciatorPanel();
                Annunciator = new Mock<AnnunciatorPanel>();
                Panel.Controls.Add(Annunciator.Object);
            };
    }
#endregion
}
