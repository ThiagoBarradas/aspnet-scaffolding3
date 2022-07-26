using AspNetScaffolding.Extensions.Logger;

using Xunit;

namespace AspNetScaffolding3Tests.Extensions.Logger
{
    public class LoggerSettingsTest
    {
        private LoggerSettings _loggerSettings;

        public LoggerSettingsTest()
        {
            _loggerSettings = new LoggerSettings();
        }

        [Fact]
        public void GetInformationTitle_InformingInformationTitle_ReturnsInformedValue()
        {
            var informationTitle = "Information Title";
            _loggerSettings.InformationTitle = informationTitle;

            var expected = informationTitle;
            var actual = _loggerSettings.GetInformationTitle();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetInformationTitle_InformationTitleEqualNull_ReturnDefaultValue()
        {
            var expected = "HTTP {Method} {Path} from {Ip} responded {StatusCode} in {ElapsedMilliseconds} ms";
            var actual = _loggerSettings.GetInformationTitle();

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void GetInformationTitle_InformationTitleEqualEmpty_ReturnDefaultValue()
        {
            var informationTitle = " ";
            _loggerSettings.InformationTitle = informationTitle;

            var expected = "HTTP {Method} {Path} from {Ip} responded {StatusCode} in {ElapsedMilliseconds} ms";
            var actual = _loggerSettings.GetInformationTitle();

            Assert.Equal(expected, actual);
        }
        
         [Fact]
        public void GetErrorTitle_ErrorTitleTitle_ReturnsInformedValue()
        {
            var errorTitle = "Error Title";
            _loggerSettings.ErrorTitle = errorTitle;

            var expected = errorTitle;
            var actual = _loggerSettings.GetErrorTitle();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetErrorTitle_ErrorTitleEqualNull_ReturnDefaultValue()
        {
            var expected = "HTTP {Method} {Path} from {Ip} responded {StatusCode} in {ElapsedMilliseconds} ms";
            var actual = _loggerSettings.GetErrorTitle();

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void GetErrorTitle_ErrorTitleEqualEmpty_ReturnDefaultValue()
        {
            var informationTitle = " ";
            _loggerSettings.ErrorTitle = informationTitle;

            var expected = "HTTP {Method} {Path} from {Ip} responded {StatusCode} in {ElapsedMilliseconds} ms";
            var actual = _loggerSettings.GetErrorTitle();

            Assert.Equal(expected, actual);
        }
    }
}