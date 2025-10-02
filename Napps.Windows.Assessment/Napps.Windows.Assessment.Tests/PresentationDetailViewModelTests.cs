using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Napps.Windows.Assessment.Domain.Model;
using Napps.Windows.Assessment.Logger;
using Napps.Windows.Assessment.Services.Interfaces;
using Napps.Windows.Assessment.ViewModels;
using System;

namespace Napps.Windows.Assessment.Tests
{
    [TestClass]
    public class PresentationDetailViewModelTests
    {
        private Mock<ILogger> _mockLogger;
        private Mock<IViewNavigationService> _mockNavigationService;
        private PresentationDetailViewModel _viewModel;

        [TestInitialize]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger>();
            _mockNavigationService = new Mock<IViewNavigationService>();

            _viewModel = new PresentationDetailViewModel(_mockLogger.Object, _mockNavigationService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Should_Throw_On_Null_Logger()
        {
            var vm = new PresentationDetailViewModel(null, _mockNavigationService.Object);
        }

        [TestMethod]
        public void Presentation_Setter_Should_Raise_PropertyChanged()
        {
            var raised = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_viewModel.Presentation)) raised = true;
            };

            _viewModel.Presentation = new Presentation("1", "Title", "thumbnailUrl", "thumbnaulFile", Privacy.Public, DateTime.UtcNow, new Author("1", "John", "Doe"), "Description");

            Assert.IsTrue(raised);
        }
    }
}
