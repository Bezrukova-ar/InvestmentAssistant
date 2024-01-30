﻿#pragma checksum "..\..\..\Pages\StatisticsPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "0CCDCBF65C0FDB89963EA20D3DC35ACEC505AD3B1A96F7418AED4E2AD6454B42"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using LiveCharts.Wpf;
using OxyPlot.Axes;
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
    /// StatisticsPage
    /// </summary>
    public partial class StatisticsPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 47 "..\..\..\Pages\StatisticsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox autoComboBox;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\Pages\StatisticsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker startDatePicker;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\Pages\StatisticsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker endDatePicker;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\Pages\StatisticsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button downloadStockPriceChart;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\Pages\StatisticsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal LiveCharts.Wpf.CartesianChart candlestickChart;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\Pages\StatisticsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal LiveCharts.Wpf.CartesianChart volumeChart;
        
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
            System.Uri resourceLocater = new System.Uri("/InvestmentAssistant;component/pages/statisticspage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\StatisticsPage.xaml"
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
            this.autoComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 51 "..\..\..\Pages\StatisticsPage.xaml"
            this.autoComboBox.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.autoComboBox_PreviewKeyDown);
            
            #line default
            #line hidden
            
            #line 52 "..\..\..\Pages\StatisticsPage.xaml"
            this.autoComboBox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.autoComboBox_PreviewTextInput);
            
            #line default
            #line hidden
            
            #line 53 "..\..\..\Pages\StatisticsPage.xaml"
            this.autoComboBox.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.autoComboBox_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.startDatePicker = ((System.Windows.Controls.DatePicker)(target));
            
            #line 57 "..\..\..\Pages\StatisticsPage.xaml"
            this.startDatePicker.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.startDatePicker_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.endDatePicker = ((System.Windows.Controls.DatePicker)(target));
            
            #line 58 "..\..\..\Pages\StatisticsPage.xaml"
            this.endDatePicker.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.endDatePicker_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.downloadStockPriceChart = ((System.Windows.Controls.Button)(target));
            
            #line 60 "..\..\..\Pages\StatisticsPage.xaml"
            this.downloadStockPriceChart.Click += new System.Windows.RoutedEventHandler(this.downloadStockPriceChart_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.candlestickChart = ((LiveCharts.Wpf.CartesianChart)(target));
            return;
            case 6:
            this.volumeChart = ((LiveCharts.Wpf.CartesianChart)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

