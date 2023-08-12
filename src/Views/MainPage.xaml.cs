using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

using StedySoft.Maui.Framework;

namespace StedySoft.BindingIssues.Views {

	public partial class MainPage : ContentPage {

		private DateTime dt = DateTime.Now;

		public readonly ObservableCollection<FontItem> FontCollection = new();

		public MainPage() {
			this.InitializeComponent();
			static string GetEnumDescriptionFunc(object value) {
				object[] attributeArray = value.GetType().GetField(value.ToString()).GetCustomAttributes(false);
				return attributeArray.Length == 0
					? value.ToString()
					: attributeArray[0].As<DescriptionAttribute>().Description;
			}
			foreach (FontFamilyNames ffn in Enum.GetValues(typeof(FontFamilyNames))) {
				this.FontCollection.Add(new FontItem {
					Name = GetEnumDescriptionFunc(ffn),
					FontFamily = ffn.ToString(),
					Value = ffn
				});
			}
			this.fontsCollectionView.ItemsSource = this.FontCollection;
			this.fontsCollectionView.SelectedItem = this.FontCollection[0];
		}

		public static readonly BindableProperty WeekDayProperty =
			BindableProperty.Create(
				nameof(MainPage.WeekDay),
				typeof(DayOfWeek),
				typeof(MainPage),
				null,
				propertyChanged: (b, o, n) => {
					MainPage mainPage = b.As<MainPage>();
					mainPage.CurrentLabelText = mainPage.WeekDay?.ToAbbreviatedDayString();
				});

		public DayOfWeek? WeekDay {
			get => (DayOfWeek)this.GetValue(MainPage.WeekDayProperty);
			set => this.SetValue(MainPage.WeekDayProperty, value);
		}

		internal static readonly BindablePropertyKey CurrentLabelTextPropertyKey =
			BindableProperty.CreateReadOnly(
				nameof(MainPage.CurrentLabelText),
				typeof(string),
				typeof(MainPage),
				null);

		public static readonly BindableProperty CurrentLabelTextProperty = CurrentLabelTextPropertyKey.BindableProperty;

		public string CurrentLabelText {
			get => (string)this.GetValue(MainPage.CurrentLabelTextProperty);
			set => this.SetValue(MainPage.CurrentLabelTextPropertyKey, value);
		}

		public static readonly BindableProperty LabelColorProperty =
			BindableProperty.Create(
				nameof(MainPage.LabelColor),
				typeof(Color),
				typeof(MainPage),
				Microsoft.Maui.Graphics.Colors.Black);

		public Color LabelColor {
			get => (Color)this.GetValue(MainPage.LabelColorProperty);
			set => this.SetValue(MainPage.LabelColorProperty, value);
		}

		private void FontSelectionChanged(object sender, SelectionChangedEventArgs e) =>
			Maui.Framework.FontManager.Current.FontFamily = this.fontsCollectionView.SelectedItem.As<FontItem>().Value;

		private void CreateLabelNoTextClicked(object sender, EventArgs e) => 
			this.LabelHolder.Children.Add(
				new Label {
					BindingContext = this,
					HorizontalTextAlignment = TextAlignment.Center,
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					LineBreakMode = LineBreakMode.NoWrap,
					VerticalTextAlignment = TextAlignment.Center,
					VerticalOptions = LayoutOptions.Center
				}.Bind(Label.TextColorProperty, nameof(this.LabelColor))
				 .Bind(Label.TextProperty, nameof(this.CurrentLabelText)));

		private void CreateLabelWithTextClicked(object sender, EventArgs e) =>
			this.LabelHolder.Children.Add(
				new Label {
					BindingContext = this,
					HorizontalTextAlignment = TextAlignment.Center,
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					Text = this.CurrentLabelText,
					TextColor = this.LabelColor,
					LineBreakMode = LineBreakMode.NoWrap,
					VerticalTextAlignment = TextAlignment.Center,
					VerticalOptions = LayoutOptions.Center
				}.Bind(Label.TextColorProperty, nameof(this.LabelColor))
				 .Bind(Label.TextProperty, nameof(this.CurrentLabelText)));

		private void OnButtonModifyWeekDayClicked(object sender, EventArgs e) {
			this.dt = dt.AddDays(1);
			this.WeekDay = dt.DayOfWeek;
		}

		private void OnButtonModifyTextColorClicked(object sender, EventArgs e) {
			if (this.LabelColor == Microsoft.Maui.Graphics.Colors.Black) {
				this.LabelColor = Microsoft.Maui.Graphics.Colors.Blue;
			}
			else if (this.LabelColor == Microsoft.Maui.Graphics.Colors.Blue) {
				this.LabelColor = Microsoft.Maui.Graphics.Colors.Green;
			}
			else if (this.LabelColor == Microsoft.Maui.Graphics.Colors.Green) {
				this.LabelColor = Microsoft.Maui.Graphics.Colors.Red;
			}
			else if (this.LabelColor == Microsoft.Maui.Graphics.Colors.Red) {
				this.LabelColor = Microsoft.Maui.Graphics.Colors.Black;
			}
		}

	}

}
