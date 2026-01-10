namespace Security
{
    public interface ILicenseProvider
    {
        bool HasValidLicense(out string reason);
    }

}