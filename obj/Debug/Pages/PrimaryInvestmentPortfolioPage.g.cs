﻿#pragma checksum "..\..\..\Pages\PrimaryInvestmentPortfolioPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "57243A7947258250B854708588E0ECC370A3B041E2D1D2F43EE7DD0289685A1D"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using InvestmentAssistant.Pages;
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


namespace InvestmentAssistant.Pages {
    
    
    /// <summary>
    /// PrimaryInvestmentPortfolioPage
    /// </summary>
    public partial class PrimaryInvestmentPortfolioPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 43 "..\..\..\Pages\PrimaryInvestmentPortfolioPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox capitalTextBox;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\Pages\PrimaryInvestmentPortfolioPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox investmentGoalComboBox;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\Pages\PrimaryInvestmentPortfolioPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox investmentHorizonComboBox;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\Pages\PrimaryInvestmentPortfolioPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox riskAccountingComboBox;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\Pages\PrimaryInvestmentPortfolioPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox expectedReturnComboBox;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\Pages\PrimaryInvestmentPortfolioPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock strategyTextBlock;
        
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
            System.Uri resourceLocater = new System.Uri("/InvestmentAssistant;component/pages/primaryinvestmentportfoliopage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\PrimaryInvestmentPortfolioPage.xaml"
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
            this.capitalTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 43 "..\..\..\Pages\PrimaryInvestmentPortfolioPage.xaml"
            this.capitalTextBox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.capitalTextBox_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 2:
            this.investmentGoalComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 45 "..\..\..\Pages\PrimaryInvestmentPortfolioPage.xaml"
            this.investmentGoalComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.investmentGoalComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.investmentHorizonComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 47 "..\..\..\Pages\PrimaryInvestmentPortfolioPage.xaml"
            this.investmentHorizonComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.investmentHorizonComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.riskAccountingComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 49 "..\..\..\Pages\PrimaryInvestmentPortfolioPage.xaml"
            this.riskAccountingComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.riskAccountingComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.expectedReturnComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 51 "..\..\..\Pages\PrimaryInvestmentPortfolioPage.xaml"
            this.expectedReturnComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.expectedReturnComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.strategyTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            
            #line 54 "..\..\..\Pages\PrimaryInvestmentPortfolioPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

