// Decompiled with JetBrains decompiler
// Type: Accordion.DefaultTemplate
// Assembly: Shelf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 34392FE8-51B4-4368-914A-8B6FB98A7971
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\Shelf.dll

using Shelf.Views;
using System;
using Xamarin.Forms;

namespace Accordion
{
  public class DefaultTemplate : AbsoluteLayout
  {
    public DefaultTemplate()
    {
      ((Layout) this).Padding = new Thickness(5.0, 2.0, 5.0, 2.0);
      ((VisualElement) this).HeightRequest = 100.0;
      ((View) new Label()
      {
        HorizontalTextAlignment = (TextAlignment) 0
      }).HorizontalOptions = LayoutOptions.StartAndExpand;
      ((View) new Label()
      {
        HorizontalTextAlignment = (TextAlignment) 2
      }).HorizontalOptions = LayoutOptions.End;
      Button button1 = new Button();
      ((View) button1).HorizontalOptions = LayoutOptions.Fill;
      button1.FontSize = 20.0;
      ((VisualElement) button1).BackgroundColor = Color.FromRgb(83, 186, 157);
      button1.TextColor = Color.White;
      Button button2 = button1;
      button2.Clicked += new EventHandler(this.btn_Clicked);
      this.Children.Add((View) button2, new Rectangle(0.0, 0.5, 1.0, 1.0), (AbsoluteLayoutFlags) -1);
      BindableObjectExtensions.SetBinding((BindableObject) button2, Button.TextProperty, "Title", (BindingMode) 0, (IValueConverter) null, (string) null);
    }

    public async void btn_Clicked(object sender, EventArgs e)
    {
      DefaultTemplate defaultTemplate = this;
      Button button = (Button) sender;
      ContentPage contentPage = new ContentPage();
      string text = button.Text;
      if (!(text == "Ürün Topla"))
      {
        if (!(text == "Sepet Okut"))
        {
          if (!(text == "Raf Girişi(Serbest)"))
          {
            if (!(text == "Raf Sayım"))
            {
              if (text == "Raflar Arası Transfer")
                contentPage = (ContentPage) new Picking();
            }
            else
              contentPage = (ContentPage) new Picking();
          }
          else
            contentPage = (ContentPage) new ShelfEntry();
        }
        else
          contentPage = (ContentPage) new Basket();
      }
      else
        contentPage = (ContentPage) new Picking();
      await ((NavigableElement) defaultTemplate).Navigation.PushAsync((Page) contentPage);
    }
  }
}
