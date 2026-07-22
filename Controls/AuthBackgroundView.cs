using Microsoft.Maui.Controls;

namespace Water_reminder.Controls;

public class AuthBackgroundView : Image
{
    public AuthBackgroundView()
    {
        Source = "background.jpg"; // altere aqui o papel de parede do fundo do aplicativo, atente-se para o nome e a extensão do arquivo

        Aspect = Aspect.AspectFill;

        HorizontalOptions = LayoutOptions.Fill;

        VerticalOptions = LayoutOptions.Fill;

        InputTransparent = true;
    }
}