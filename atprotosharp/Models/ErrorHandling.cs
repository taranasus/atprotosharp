namespace atprotosharp;

public class ErrorHandling
{
    public ErrorHandling()
    {
        this.success = true;
        this.errorMessage = null;
    }
    public ErrorHandling(bool success, string errorMessage)
    {
        this.success = success;
        this.errorMessage = errorMessage;
    }

    public bool success { get; set; }
    public string errorMessage { get; set; }
}