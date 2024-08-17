using TMPro;

public class Wallet
{
    private TextMeshProUGUI _textView;
    private int _value = 0;

    public void Add()
    {
        _value++;
    }

    public void Show()
    {
        string phrase = "Собрано осколков сознания: ";

        _textView.text = phrase + _value.ToString();
    }

    public void SetTextMeshProUGUI(TextMeshProUGUI textView)
    {
        _textView = textView;
    }
}
