﻿#pragma checksum "..\..\EyeCareShortWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5A761A05C3A2D9055193B8C5938D62BD5362789B474E206F881D0EA0B77F9554"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using EyeCarePC;
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


namespace EyeCarePC {
    
    
    /// <summary>
    /// EyeCareShortWindow
    /// </summary>
    public partial class EyeCareShortWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 63 "..\..\EyeCareShortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CloseBtn;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\EyeCareShortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label TimeLeft;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\EyeCareShortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LongBreakImage;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\EyeCareShortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas EyeCanvas;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\EyeCareShortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Ellipse LeftEye;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\EyeCareShortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Ellipse LeftPupil;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\EyeCareShortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Ellipse RightEye;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\EyeCareShortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Ellipse RightPupil;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\EyeCareShortWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock description;
        
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
            System.Uri resourceLocater = new System.Uri("/EyeCarePC;component/eyecareshortwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\EyeCareShortWindow.xaml"
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
            
            #line 8 "..\..\EyeCareShortWindow.xaml"
            ((EyeCarePC.EyeCareShortWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 8 "..\..\EyeCareShortWindow.xaml"
            ((EyeCarePC.EyeCareShortWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.CloseBtn = ((System.Windows.Controls.Button)(target));
            
            #line 63 "..\..\EyeCareShortWindow.xaml"
            this.CloseBtn.Click += new System.Windows.RoutedEventHandler(this.CloseButton_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.TimeLeft = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.LongBreakImage = ((System.Windows.Controls.Grid)(target));
            return;
            case 5:
            this.EyeCanvas = ((System.Windows.Controls.Canvas)(target));
            return;
            case 6:
            this.LeftEye = ((System.Windows.Shapes.Ellipse)(target));
            return;
            case 7:
            this.LeftPupil = ((System.Windows.Shapes.Ellipse)(target));
            return;
            case 8:
            this.RightEye = ((System.Windows.Shapes.Ellipse)(target));
            return;
            case 9:
            this.RightPupil = ((System.Windows.Shapes.Ellipse)(target));
            return;
            case 10:
            this.description = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

