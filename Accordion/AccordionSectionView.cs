// Decompiled with JetBrains decompiler
// Type: Accordion.AccordionSectionView
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Accordion
{
  public class AccordionSectionView : StackLayout
  {
    private bool _isExpanded;
    private StackLayout _content;
    private Color _headerColor;
    private ImageSource _arrowRight;
    private ImageSource _arrowDown;
    private AbsoluteLayout _header;
    private Image _headerIcon;
    private Label _headerTitle;
    private DataTemplate _template;
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof (ItemsSource), typeof (IList), typeof (AccordionSectionView), (object) null, (BindingMode) 2, (BindableProperty.ValidateValueDelegate) null, new BindableProperty.BindingPropertyChangedDelegate((object) null, __methodptr(PopulateList)), (BindableProperty.BindingPropertyChangingDelegate) null, (BindableProperty.CoerceValueDelegate) null, (BindableProperty.CreateDefaultValueDelegate) null);
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof (Title), typeof (string), typeof (AccordionSectionView), (object) null, (BindingMode) 2, (BindableProperty.ValidateValueDelegate) null, new BindableProperty.BindingPropertyChangedDelegate((object) null, __methodptr(ChangeTitle)), (BindableProperty.BindingPropertyChangingDelegate) null, (BindableProperty.CoerceValueDelegate) null, (BindableProperty.CreateDefaultValueDelegate) null);

    public IList ItemsSource
    {
      get => (IList) ((BindableObject) this).GetValue(AccordionSectionView.ItemsSourceProperty);
      set => ((BindableObject) this).SetValue(AccordionSectionView.ItemsSourceProperty, (object) value);
    }

    public string Title
    {
      get => (string) ((BindableObject) this).GetValue(AccordionSectionView.TitleProperty);
      set => ((BindableObject) this).SetValue(AccordionSectionView.TitleProperty, (object) value);
    }

    public AccordionSectionView(DataTemplate itemTemplate, ScrollView parent)
    {
      StackLayout stackLayout = new StackLayout();
      ((VisualElement) stackLayout).HeightRequest = 0.0;
      this._content = stackLayout;
      this._headerColor = Color.FromRgb(83, 186, 157);
      this._arrowRight = ImageSource.FromFile("ic_keyboard_arrow_right_white_24dp.png");
      this._arrowDown = ImageSource.FromFile("ic_keyboard_arrow_down_white_24dp.png");
      this._header = new AbsoluteLayout();
      Image image = new Image();
      ((View) image).VerticalOptions = LayoutOptions.Center;
      this._headerIcon = image;
      Label label = new Label();
      label.TextColor = Color.White;
      label.FontAttributes = (FontAttributes) 1;
      label.VerticalTextAlignment = (TextAlignment) 1;
      label.FontSize = 20.0;
      ((VisualElement) label).HeightRequest = 70.0;
      this._headerTitle = label;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      AccordionSectionView accordionSectionView = this;
      this._template = itemTemplate;
      ((VisualElement) this._headerTitle).BackgroundColor = this._headerColor;
      this._headerIcon.Source = this._arrowRight;
      ((VisualElement) this._header).BackgroundColor = this._headerColor;
      this._header.Children.Add((View) this._headerIcon, new Rectangle(0.0, 1.0, 0.1, 1.0), (AbsoluteLayoutFlags) -1);
      this._header.Children.Add((View) this._headerTitle, new Rectangle(1.0, 1.0, 0.9, 1.0), (AbsoluteLayoutFlags) -1);
      this.Spacing = 0.0;
      ((ICollection<View>) ((Layout<View>) this).Children).Add((View) this._header);
      ((ICollection<View>) ((Layout<View>) this).Children).Add((View) this._content);
      ((ICollection<IGestureRecognizer>) ((View) this._header).GestureRecognizers).Add((IGestureRecognizer) new TapGestureRecognizer()
      {
        Command = (ICommand) new Command((Action) (() =>
        {
          if (accordionSectionView._isExpanded)
          {
            accordionSectionView._headerIcon.Source = accordionSectionView._arrowRight;
            ((VisualElement) accordionSectionView._content).HeightRequest = 0.0;
            ((VisualElement) accordionSectionView._content).IsVisible = false;
            accordionSectionView._isExpanded = false;
          }
          else
          {
            accordionSectionView._headerIcon.Source = accordionSectionView._arrowDown;
            ((VisualElement) accordionSectionView._content).HeightRequest = (double) (((ICollection<View>) ((Layout<View>) accordionSectionView._content).Children).Count * 100);
            ((VisualElement) accordionSectionView._content).IsVisible = true;
            accordionSectionView._isExpanded = true;
            if (!(((Element) parent).Parent is VisualElement))
              return;
            await parent.ScrollToAsync(0.0, ((VisualElement) accordionSectionView).Y, true);
          }
        }))
      });
    }

    private void ChangeTitle()
    {
      string title = this.Title;
      this._headerTitle.Text = this.Title;
    }

    private void PopulateList()
    {
      ((ICollection<View>) ((Layout<View>) this._content).Children).Clear();
      foreach (object obj in (IEnumerable) this.ItemsSource)
      {
        View content = (View) ((ElementTemplate) this._template).CreateContent();
        ((BindableObject) content).BindingContext = obj;
        ((ICollection<View>) ((Layout<View>) this._content).Children).Add(content);
      }
    }

    private static void ChangeTitle(BindableObject bindable, object oldValue, object newValue)
    {
      if (oldValue == newValue)
        return;
      ((AccordionSectionView) bindable).ChangeTitle();
    }

    private static void PopulateList(BindableObject bindable, object oldValue, object newValue)
    {
      if (oldValue == newValue)
        return;
      ((AccordionSectionView) bindable).PopulateList();
    }
  }
}
