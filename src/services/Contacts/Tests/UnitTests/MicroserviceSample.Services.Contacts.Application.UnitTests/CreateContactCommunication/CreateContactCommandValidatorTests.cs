using FluentValidation.Results;
using MicroserviceSample.Services.Contacts.Application.Contacts.CreateContactCommunication;

namespace Services.Contacts.Application.UnitTests.CreateContactCommunication
{
    public class CreateContactCommunicationCommandValidatorTests
    {
        [Fact]
        public void ValidateContactCommunicationCommand_WhenTypeEmpty_IsFail()
        {
            // Arrange 
            CreateContactCommunicationCommandValidator validator = new ();
            
            //Act
            ValidationResult? validateResult =
                validator.Validate(
                    new CreateContactCommunicationCommand(Guid.NewGuid(), string.Empty, It.IsAny<string>()));
           
            //Assert
            validateResult.Should().NotBeNull();
            validateResult.Errors.Should().HaveCount(1);
        }

        [Fact]
        public void ValidateContactCommunicationCommand_WhenTypeEmpty_IsSuccess()
        {
            // Arrange 
            CreateContactCommunicationCommandValidator validator = new();
            
            //Act
            ValidationResult? validateResult =
                validator.Validate(
                    new CreateContactCommunicationCommand(Guid.NewGuid(), "location", It.IsAny<string>()));
           
            //Assert
            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
        } 
    }
}