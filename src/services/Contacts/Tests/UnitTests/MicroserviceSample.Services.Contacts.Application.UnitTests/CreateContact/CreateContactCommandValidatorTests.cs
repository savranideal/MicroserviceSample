

using FluentValidation.Results;
using MicroserviceSample.Services.Contacts.Application.Contacts.CreateContact;

namespace Services.Contacts.Application.UnitTests.CreateContact
{
    public class CreateContactCommandValidatorTests
    {
        [Fact]
        public void ValidateContactCommand_WhenFirstNameEmpty_IsFail()
        {
            // Arrange 
            CreateContactCommandValidator validator = new CreateContactCommandValidator();
            
            //Act
            ValidationResult? validateResult=validator.Validate(new CreateContactCommand(string.Empty, It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<List<(string type, string value)>>()));
           
            //Assert
            validateResult.Should().NotBeNull();
            validateResult.Errors.Should().HaveCount(1);
        }

        [Theory,AutoData]
        public void ValidateContactCommand_WhenFirstNameIsValid_IsSuccess(string firtname)
        {
            // Arrange 
            CreateContactCommandValidator validator = new CreateContactCommandValidator();
            
            //Act
            ValidationResult? validateResult=validator.Validate(new CreateContactCommand(firtname, It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<List<(string type, string value)>>()));
           
            //Assert
            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
        }
    }
}