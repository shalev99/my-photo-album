using Moq;
using Microsoft.AspNetCore.Mvc;
using PictureAlbum.API.Controllers;
using PictureAlbum.API.Models;
using PictureAlbum.API.Services;
using Xunit;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

public class FileControllerTests
{
    private readonly Mock<IFileService> _fileServiceMock;
    private readonly FileController _controller;

    public FileControllerTests()
    {
        _fileServiceMock = new Mock<IFileService>();
        _controller = new FileController(_fileServiceMock.Object);
    }

    [Fact]
    public async Task UploadFile_ReturnsOkResult_WhenFileIsUploaded()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        var fileName = "test.jpg";
        var fileDate = "2025-02-21";
        var fileDescription = "Test file";

        var fileEntity = new FileEntity
        {
            Id = 1,  // Updated to int
            Name = fileName,
            FileName = fileName,
            FileType = "image/jpeg",
            FileContentBase64 = "base64string",
            FileContent = new byte[0], // Just to represent empty byte array
            UploadDate = DateTime.Now
        };

        // Ensure mock setup returns the fileEntity object correctly
        _fileServiceMock.Setup(f => f.UploadFileAsync(mockFile.Object, fileName, fileDate, fileDescription))
            .ReturnsAsync(fileEntity);  // Return the actual FileEntity object

        // Act
        var result = await _controller.UploadFile(mockFile.Object, fileName, fileDate, fileDescription);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<FileEntity>(okResult.Value); // Ensure it matches FileEntity
        Assert.Equal(fileEntity.Id, returnValue.Id); // Ensure the ID matches
        Assert.Equal(fileEntity.Name, returnValue.Name); // Ensure the Name matches
    }

    [Fact]
    public async Task UploadFile_ReturnsServerError_WhenExceptionOccurs()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        var fileName = "test.jpg";
        var fileDate = "2025-02-21";
        var fileDescription = "Test file";

        // Simulate an exception in the file upload service
        _fileServiceMock.Setup(f => f.UploadFileAsync(mockFile.Object, fileName, fileDate, fileDescription))
            .ThrowsAsync(new Exception("Error uploading file"));

        // Act
        var result = await _controller.UploadFile(mockFile.Object, fileName, fileDate, fileDescription);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode); // Assert that the server error is returned
    }

    [Fact]
    public async Task GetFiles_ReturnsOkResult_WhenFilesAreFound()
    {
        // Arrange
        var files = new List<FileEntity> { new FileEntity { Id = 1, Name = "file1" } };
        _fileServiceMock.Setup(f => f.GetFilesAsync(1, 10)).ReturnsAsync(files);

        // Act
        var result = await _controller.GetFiles(1, 10);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<FileEntity>>(okResult.Value);
        Assert.Single(returnValue); // Assert that only one file is returned
    }

    [Fact]
    public async Task GetFiles_ReturnsServerError_WhenExceptionOccurs()
    {
        // Arrange
        _fileServiceMock.Setup(f => f.GetFilesAsync(1, 10)).ThrowsAsync(new Exception("Error fetching files"));

        // Act
        var result = await _controller.GetFiles(1, 10);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode); // Assert that a server error is returned
    }
}
