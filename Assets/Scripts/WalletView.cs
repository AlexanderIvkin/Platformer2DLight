using TMPro;

public class WalletView 
{
    private TextMeshProUGUI _textView;
    private Wallet _wallet;
    private string _phrase = "Собрано осколков кошачьего сознания: ";

    public WalletView(Wallet wallet, TextMeshProUGUI textView)
    {
        _wallet = wallet;
        _textView = textView;
    }

    public void Show()
    {
        _textView.text = _phrase + _wallet.Count;
    }
}
