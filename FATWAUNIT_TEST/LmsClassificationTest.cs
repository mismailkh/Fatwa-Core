using FATWA_API.Controllers.V1;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FATWAUNIT_TEST
{
    public class LmsClassificationTest
    {
        #region Property  
        public Mock<ILmsLiteratureClassification> mock = new Mock<ILmsLiteratureClassification>();
        public LmsLiteratureClassification? firstEmployee = null;

        #endregion

        [Fact]
        public async void GetClassificationDetails()
        {
            var classificationDTO = new LmsLiteratureClassification()
            {
                ClassificationId = 4,
                Name_En = "Aqeel",
                Name_Ar = "عقيل",
                CreatedBy = "superadmin@gmail.com",
                CreatedDate = Convert.ToDateTime("2022-03-17 10:18:18.187"),
                ModifiedBy = null,
                ModifiedDate = null,
                DeletedBy = "",
                DeletedDate = null,
                IsDeleted = false
            };
            mock.Setup(p => p.GetLiteratureClassificationDetailById(4)).ReturnsAsync(classificationDTO);
            LmsLiteratureClassificationsController emp = new LmsLiteratureClassificationsController(mock.Object);
            var result = await emp.Get(1);
            Assert.True(classificationDTO.Equals(result));
        }
        [Fact]
        public void GetEmployee_ListOfEmployee_EmployeeExistsInRepo()
        {
            //arrange
            var employee = GetSampleEmployee();
            mock.Setup(x => x.GetLmsLiteratureClassifications())
                .Returns(GetSampleEmployee);
            var controller = new LmsLiteratureClassificationsController(mock.Object);

            //act
            var actionResult = controller.GetLmsLiteratureClassifications();
            var result = actionResult.Result as OkObjectResult;
            var actual = result.Value as IEnumerable<LmsLiteratureClassification>;

            //assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(GetSampleEmployee().Count(), actual.Count());
        }
        //[Fact]
        //public void GetEmployeeById_EmployeeObject_EmployeewithSpecificeIdExists()
        //{
        //    //arrange
        //    List<LmsLiteratureClassification> employees = GetSampleEmployee();
        //    firstEmployee = employees[0];
        //    mock.Setup(x => x.GetLiteratureClassificationDetailById(1))
        //        .Returns(firstEmployee);
        //    var controller = new LmsLiteratureClassificationsController(mock.Object);

        //    //act
        //    var actionResult = controller.Get(1);
        //    var result = actionResult.Result as OkObjectResult;

        //    //Assert
        //    Assert.IsType<OkObjectResult>(result);

        //    result.Value.Equals(firstEmployee);
        //}



        [Theory]
        [InlineData(18)]
        [InlineData(20)]
        public void checkIfUserCanBeVoter_true_ageGreaterThan18(int age)
        {
            ////arrange
            //var controller = new EmployeeController(null);

            ////act
            //var actual = controller.checkIfUserCanBeVoter(age);

            ////Assert
            //Assert.True(actual);

        }

        [Theory]
        [InlineData(17)]
        [InlineData(15)]
        public void checkIfUserCanBeVoter_true_ageLessThan18(int age)
        {
            //arrange
            //var controller = new EmployeeController(null);

            ////act
            //var actual = controller.checkIfUserCanBeVoter(age);

            ////Assert
            //Assert.False(actual);

        }
        private List<LmsLiteratureClassification> GetSampleEmployee()
        {
            List<LmsLiteratureClassification> output = new List<LmsLiteratureClassification>
        {
            new LmsLiteratureClassification
            {
                ClassificationId = 4,
                Name_En = "Aqeel",
                Name_Ar = "عقيل",
                CreatedBy = "superadmin@gmail.com",
                CreatedDate = Convert.ToDateTime("2022-03-17 10:18:18.187"),
                ModifiedBy = null,
                ModifiedDate = null,
                DeletedBy = "",
                DeletedDate = null,
                IsDeleted = false
            },
            new LmsLiteratureClassification
            {
                ClassificationId = 5,
                Name_En = "TestReloa",
                Name_Ar = "عقيل",
                CreatedBy = "superadmin@gmail.com",
                CreatedDate = Convert.ToDateTime("2022-03-17 10:24:23.423"),
                ModifiedBy = null,
                ModifiedDate = null,
                DeletedBy = "",
                DeletedDate = null,
                IsDeleted = false
            },
            new LmsLiteratureClassification
            {

                ClassificationId = 6,
                Name_En = "tessst",
                Name_Ar = "test33",
                CreatedBy = "superadmin@gmail.com",
                CreatedDate = Convert.ToDateTime("2022-03-17 10:30:45.263"),
                ModifiedBy = null,
                ModifiedDate = null,
                DeletedBy = "",
                DeletedDate = null,
                IsDeleted = false
            }
        };
            return output;
        }
    }

}
