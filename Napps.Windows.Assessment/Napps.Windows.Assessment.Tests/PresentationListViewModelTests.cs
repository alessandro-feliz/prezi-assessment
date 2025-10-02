using Caliburn.Micro;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Napps.Windows.Assessment.Domain.Model;
using Napps.Windows.Assessment.Logger;
using Napps.Windows.Assessment.Repositories.Presentations.Interfaces;
using Napps.Windows.Assessment.Services.Interfaces;
using Napps.Windows.Assessment.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Napps.Windows.Assessment.Tests
{
    [TestClass]
    public class PresentationListViewModelTests
    {
        private Mock<ILogger> _mockLogger;
        private Mock<IViewNavigationService> _mockNavigationService;
        private Mock<IEventAggregator> _mockEventAggregator;
        private Mock<IBusyIndicatorService> _mockBusyIndicator;
        private Mock<IPresentationReader> _mockFallbackPresentationRepository;
        private PresentationListViewModel _viewModel;

        [TestInitialize]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger>();
            _mockNavigationService = new Mock<IViewNavigationService>();
            _mockEventAggregator = new Mock<IEventAggregator>();
            _mockBusyIndicator = new Mock<IBusyIndicatorService>();
            _mockFallbackPresentationRepository = new Mock<IPresentationReader>();

            _viewModel = new PresentationListViewModel(_mockLogger.Object, _mockNavigationService.Object, _mockEventAggregator.Object, _mockBusyIndicator.Object, _mockFallbackPresentationRepository.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Should_Throw_On_Null_BusyIndicador()
        {
            var vm = new PresentationListViewModel(_mockLogger.Object, _mockNavigationService.Object, _mockEventAggregator.Object, null, _mockFallbackPresentationRepository.Object);
        }

        [TestMethod]
        public async Task InitializeAsync_Should_Order_Presentations_By_LastModified_Descending()
        {
            var presentations = new[]
            {
               new Presentation("1", "Title 1", "thumbnailUrl", "thumbnaulFile", Privacy.Public, new DateTime(2023, 1, 1), new Author("1", "John", "Doe"), "Description"),
               new Presentation("2", "Title 2", "thumbnailUrl", "thumbnaulFile", Privacy.Public, new DateTime(2024, 1, 1), new Author("1", "John", "Doe"), "Description")
            };

            _mockFallbackPresentationRepository.Setup(r => r.LoadAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PresentationsLoadResult(Mode.Online, presentations));

            await _viewModel.InitializeAsync(CancellationToken.None);

            // Assert
            Assert.AreEqual(2, _viewModel.Presentations.Count);
            Assert.AreEqual("2", _viewModel.Presentations[0].Id);
            Assert.AreEqual("1", _viewModel.Presentations[1].Id);
        }


        [TestMethod]
        public async Task InitializeAsync_Should_ShowBusyIndicator_And_LoadPresentations_Successfully()
        {
            var presentations = new[]
            {
               new Presentation("1", "Title 1", "thumbnailUrl", "thumbnaulFile", Privacy.Public, new DateTime(2023, 1, 1), new Author("1", "John", "Doe"), "Description"),
               new Presentation("2", "Title 2", "thumbnailUrl", "thumbnaulFile", Privacy.Public, new DateTime(2024, 1, 1), new Author("1", "John", "Doe"), "Description")
            };

            _mockFallbackPresentationRepository.Setup(r => r.LoadAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PresentationsLoadResult(Mode.Online, presentations));

            await _viewModel.InitializeAsync(CancellationToken.None);

            // Assert
            _mockBusyIndicator.Verify(b => b.Show(), Times.Once);
            _mockBusyIndicator.Verify(b => b.Hide(), Times.Once);

            Assert.AreEqual(2, _viewModel.Presentations.Count);
            Assert.AreEqual("2", _viewModel.Presentations.First().Id);

            _mockLogger.VerifyNoOtherCalls();
        }
    }
}