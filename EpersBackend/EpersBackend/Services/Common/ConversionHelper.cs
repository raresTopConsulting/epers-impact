namespace EpersBackend.Services.Common
{
    public class ConversionHelper : IConversionHelper
    {
        public int[] ConvertStringWithCommaSeparatorToIntArray(string input)
        {
            return input.TrimEnd(',').Split(',').Select(str => int.Parse(str.Trim())).ToArray();
        }
    }
}
