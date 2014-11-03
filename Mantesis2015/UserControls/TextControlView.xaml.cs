using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Mantesis2015.UserControls
{
    /// <summary>
    /// Lógica de interacción para TextControlView.xaml
    /// </summary>
    public partial class TextControlView : UserControl
    {
        public TextControlView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty MainTextControlProperty =
            DependencyProperty.Register("MainText", typeof(String),
                typeof(TextControlView), new FrameworkPropertyMetadata(string.Empty));

        public String MainText
        {
            get
            {
                return GetValue(MainTextControlProperty).ToString();
            }
            set
            {
                SetValue(MainTextControlProperty, value);
            }
        }

        public static readonly DependencyProperty LabelHeaderControlProperty =
            DependencyProperty.Register("LabelHeader", typeof(String),
                typeof(TextControlView), new FrameworkPropertyMetadata(string.Empty));

        public String LabelHeader
        {
            get
            {
                return GetValue(MainTextControlProperty).ToString();
            }
            set
            {
                SetValue(MainTextControlProperty, value);
            }
        }

        public static readonly DependencyProperty NoteControlProperty =
            DependencyProperty.Register("Note", typeof(String),
                typeof(TextControlView), new FrameworkPropertyMetadata(string.Empty));

        public String Note
        {
            get
            {
                return GetValue(MainTextControlProperty).ToString();
            }
            set
            {
                SetValue(MainTextControlProperty, value);
            }
        }
    }
}