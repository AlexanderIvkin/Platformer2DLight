using TMPro;

public class WalletView 
{
    private TextMeshProUGUI _textView;

    public WalletView(TextMeshProUGUI textView)
    {
        _textView = textView;
    }

    public void Show(int count)
    {
        _textView.text = count.ToString();
    }
}
