﻿#pragma checksum "..\..\..\MateriasSga\MateriasSgaWin.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "73D1CC0F663F00716FF46EACF6BC4B9F"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.18034
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Windows.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Mantesis2015.MateriasSga {
    
    
    /// <summary>
    /// MateriasSgaWin
    /// </summary>
    public partial class MateriasSgaWin : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 47 "..\..\..\MateriasSga\MateriasSgaWin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnQuitar;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\MateriasSga\MateriasSgaWin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnSalvar;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\MateriasSga\MateriasSgaWin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnSalir;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\..\MateriasSga\MateriasSgaWin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView tree;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Mantesis2015;component/materiassga/materiassgawin.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MateriasSga\MateriasSgaWin.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 7 "..\..\..\MateriasSga\MateriasSgaWin.xaml"
            ((Mantesis2015.MateriasSga.MateriasSgaWin)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.BtnQuitar = ((System.Windows.Controls.Button)(target));
            return;
            case 3:
            this.BtnSalvar = ((System.Windows.Controls.Button)(target));
            
            #line 60 "..\..\..\MateriasSga\MateriasSgaWin.xaml"
            this.BtnSalvar.Click += new System.Windows.RoutedEventHandler(this.BtnSalvar_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.BtnSalir = ((System.Windows.Controls.Button)(target));
            
            #line 70 "..\..\..\MateriasSga\MateriasSgaWin.xaml"
            this.BtnSalir.Click += new System.Windows.RoutedEventHandler(this.BtnSalir_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.tree = ((System.Windows.Controls.TreeView)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

