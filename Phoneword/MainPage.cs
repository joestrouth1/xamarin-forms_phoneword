using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Core;

namespace Phoneword
{
    public class MainPage : ContentPage
    {
        string translatedNumber;

        Entry entry;
        Button translateButton;
        Button callButton;

        public MainPage()
        {
            Padding = new Thickness(20);

            StackLayout panel = new StackLayout
            {
                Spacing = 15
            };

            panel.Children.Add(entry = new Entry { Text = "1-855-XAMARIN" });
            panel.Children.Add(translateButton = new Button { Text = "Translate" });
            panel.Children.Add(callButton = new Button { Text = "Call", IsEnabled = false });

            translateButton.Clicked += OnTranslate;
            callButton.Clicked += OnCall;

            Content = panel;
        }

        private void OnTranslate(object sender, EventArgs e)
        {
            var userInput = entry.Text;
            translatedNumber = PhonewordTranslator.ToNumber(userInput);
            if (!string.IsNullOrEmpty(translatedNumber))
            {
                callButton.Text = "Call " + translatedNumber;
                callButton.IsEnabled = true;
            }
            else
            {
                callButton.Text = "Call";
                callButton.IsEnabled = false;
            }
        }

        async private void OnCall(object sender, EventArgs e)
        {
            if (await DisplayAlert(
                "Dial a Number",
                "Would you like to call " + translatedNumber + "?",
                "Call",
                "Cancel"))
            {
                try
                {
                    PhoneDialer.Open(translatedNumber);
                }
                catch (ArgumentNullException)
                {
                    await DisplayAlert("Unable to dial", "Phone number was not valid.", "OK");
                }
                catch (FeatureNotSupportedException)
                {
                    await DisplayAlert("Unable to dial", "Phone dialing not supported.", "OK");
                }
                catch (Exception)
                {
                    // uh-oh
                    await DisplayAlert("Unable to dial", "Phone dialing failed", "OK");
                }
            }
        }
    }
}