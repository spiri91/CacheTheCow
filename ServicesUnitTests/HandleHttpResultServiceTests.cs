using System.Collections.Generic;
using DataObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using System.Linq;

public class Employee
{
    public double id;
    public string employee_name;
    public double employee_salary;
    public double employee_age;
}

[TestClass]
public class HandleHttpResultServiceTests
{
    private string employeesAsJson =
        "[{\"id\":\"2053\",\"employee_name\":\"diksatesthshasjhd\",\"employee_salary\":\"12301\",\"employee_age\":\"23\",\"profile_image\":\"\"},{\"id\":\"2055\",\"employee_name\":\"testthuongle111abcccc\",\"employee_salary\":\"1231111\",\"employee_age\":\"29\",\"profile_image\":\"\"},{\"id\":\"2061\",\"employee_name\":\"testthuongle111abcccc\",\"employee_salary\":\"1231112\",\"employee_age\":\"23\",\"profile_image\":\"\"},{\"id\":\"2062\",\"employee_name\":\"testthuongle111abcccc\",\"employee_salary\":\"1231111\",\"employee_age\":\"23\",\"profile_image\":\"\"},{\"id\":\"2063\",\"employee_name\":\"testthuongle111abcccc\",\"employee_salary\":\"1231111\",\"employee_age\":\"23\",\"profile_image\":\"\"},{\"id\":\"2064\",\"employee_name\":\"testthuongle111abcccc\",\"employee_salary\":\"1239999\",\"employee_age\":\"23\",\"profile_image\":\"\"}]";

    [TestMethod]
    public void ShouldReturnValidArrayOnValidResponse()
    {
        var sut = new HandleResponseService();

        var httpResponse = new HttpResponse(true, employeesAsJson, "random url");

        var result = sut.HandleArrayResult<Employee>(httpResponse, new List<Employee>()).ToList();

        Assert.IsTrue(result.Count == 6);
        Assert.IsTrue(result.Exists(x => x.id == 2063));
        Assert.IsTrue(result.Exists(x => x.employee_name == "diksatesthshasjhd"));
        Assert.IsTrue(result.Exists(x => x.employee_age == 29));
        Assert.IsTrue(result.Exists(x => x.employee_salary == 1239999));
    }

    [TestMethod]
    public void ShouldReturnDefaultObjectOnMissingResponse()
    {
        var sut = new HandleResponseService();

        var httpResponse = new HttpResponse(true, null, "random url");
        var testEmployee = new Employee() { id = 99, employee_name = "mamaliga" };

        var result = sut.HandleArrayResult<Employee>(httpResponse, new List<Employee>() { testEmployee }).ToList();

        Assert.IsTrue(result.Contains(testEmployee));
    }

    [TestMethod]
    public void ShouldReturnDefaultObjectOnInvalidResponse()
    {
        var sut = new HandleResponseService();

        var httpResponse = new HttpResponse(false, employeesAsJson, "random url");
        var testEmployee = new Employee() { id = 99, employee_name = "mamaliga" };

        var result = sut.HandleArrayResult<Employee>(httpResponse, new List<Employee>() { testEmployee }).ToList();

        Assert.IsTrue(result.Contains(testEmployee));
    }

    [TestMethod]
    public void ShouldReturnDefaultObjectOnErrorWhileParsing()
    {
        var sut = new HandleResponseService();

        var httpResponse = new HttpResponse(false, "garbageinfront==[{\"id\":\"2053\",\"employee_name\":\"diksatesthshasjhd\",\"employee_salary\":\"12301\",\"employee_age\":\"23\",\"profile_image\":\"\"}]", "random url");
        var testEmployee = new Employee() { id = 99, employee_name = "mamaliga" };

        var result = sut.HandleArrayResult<Employee>(httpResponse, new List<Employee>() { testEmployee }).ToList();

        Assert.IsTrue(result.Contains(testEmployee));
    }

    [TestMethod]
    public void ShouldReturnDefaultObjectOnNullResult()
    {
        var sut = new HandleResponseService();

        var testEmployee = new Employee() { id = 99, employee_name = "mamaliga" };
        var result = sut.HandleArrayResult<Employee>(null, new List<Employee>() { testEmployee }).ToList();

        Assert.IsTrue(result.Contains(testEmployee));
    }
}