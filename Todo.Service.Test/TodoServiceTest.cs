using MapsterMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Todo.Infrastructure.Interface;
using Todo.Infrastructure.Models.Entities;
using Todo.Service.Interface;
using Todo.Service.Models.DTOs;
using Todo.Service.Services;

namespace Todo.Service.Test
{
    [TestFixture]
    public class TodoServiceTest
    {
        #region Property  
        private Mock<ITodoItemRepository> _mockTodoRepository;
        private ITodoItemService _todoItemService;
        private IMapper _iMapper;

        #endregion
        [SetUp]
        public void SetUp()
        {
            //System.Diagnostics.Debugger.Launch();
            _iMapper = new Mapper();
            _mockTodoRepository = new Mock<ITodoItemRepository>();
            _todoItemService = new TodoItemService(_mockTodoRepository.Object, _iMapper);
        }

        //[Test]
        //public async Task CreateTodo_ReturnId()
        //{
        //    TodoItemDto testDataDto = new TodoItemDto()
        //    {
        //        Id = 1,
        //        Name = "test",
        //        Description = "Desc",
        //        DueDate = DateTime.Now.AddDays(1),
        //        Data = new { Test = "test date" }

        //    };
        //    var testData = _iMapper.Map<TodoItem>(testDataDto);
        //    _mockTodoRepository.Setup(p => p.CreateAsync(testData)).ReturnsAsync(testData.Id);
        //    int? result = await _todoItemService.CreateAsync(testDataDto);
        //    Assert.AreEqual(testDataDto.Id, result);
        //}

        [TestCaseSource(nameof(GetTodoList))]
        public async Task GetTodo_ReturnCountMatch(IEnumerable<TodoItemDto> data)
        {
            var testData = _iMapper.Map<List<TodoItem>>(data);
            _mockTodoRepository.Setup(p => p.GetAsync(default)).ReturnsAsync(testData);
            IEnumerable<TodoItemDto> result = await _todoItemService.GetAsync(null, null);
            Assert.AreEqual(data.Count(), result.Count());
        }
        private static IEnumerable<List<TodoItemDto>> GetTodoList()
        {
            yield return new List<TodoItemDto>(){
                new()
                {
                    Id = 1,
                    Name = "John",
                    Description = "Buy phone",
                    DueDate = DateTime.Now.AddDays(1),
                    Data = new { Test = "test date" }
                }};
            yield return new List<TodoItemDto>(){
                new()
                {
                    Id = 1,
                    Name = "John",
                    Description = "Buy phone",
                    DueDate = DateTime.Now.AddDays(1),
                    Data = new { Test = "test date" }
                },
                new()
                {
                    Id = 1,
                    Name = "John",
                    Description = "Buy phone",
                    DueDate = DateTime.Now.AddDays(1),
                    Data = new { Test = "test date" }
                }};
            yield return new List<TodoItemDto>(){
                new()
                {
                    Id = 1,
                    Name = "Bob",
                    Description = "Buy phone",
                    DueDate = DateTime.Now.AddDays(1),
                    Data = new { Test = "test date" }
                }};
            yield return new List<TodoItemDto>();
        }
    }
}