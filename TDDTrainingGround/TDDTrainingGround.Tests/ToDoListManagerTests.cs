﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDTrainingGround.Tests
{
    [TestFixture]
    public class ToDoListManagerTests
    {
        private ToDoListManager MakeToDoListManager()
        {
            return new ToDoListManager();
        }

        [Test]
        public void Add_WhenNullToDo_ThrowsArgumentNullException()
        {
            //Arrange
            var manager = MakeToDoListManager();

            //Act&Assert
            var ex = Assert.Catch<ArgumentNullException>(() => manager.Add(null));
            StringAssert.Contains("todo needs to be provided", ex.Message);
        }

        [Test]
        public void Add_WhenDuplicatedToDo_ThrowsInvalidOperationException()
        {
            //Arrange
            var manager = MakeToDoListManager();
            manager.Add(new ToDo("Wash dishes", new DateTime(2010, 1, 1, 9, 30, 0)));

            //Act&Assert
            var ex = Assert.Catch<InvalidOperationException>(() => manager.Add(new ToDo("Wash dishes", new DateTime(2010, 1, 1, 9, 30, 0))));
            StringAssert.Contains("there is already the same todo", ex.Message);
        }

        [Test]
        public void GetAllThingsToDo_WhenEmpty_ReturnsEmptyList()
        {
            //Arrange
            var manager = MakeToDoListManager();

            //Act
            var result = manager.GetAllThingsToDo();

            //Assert
            Assert.AreEqual(new List<ToDo>(), result);
        }

        [Test]
        public void GetAllThingsToDo_WhenNotEmpty_ReturnsListOfToDos()
        {
            //Arrange
            var manager = MakeToDoListManager();
            manager.Add(new ToDo("Go out with the dog"));
            manager.Add(new ToDo("Wash dishes"));

            //Act
            var result = manager.GetAllThingsToDo();

            var expected = new List<ToDo>
            {
                new ToDo("Go out with the dog"),
                new ToDo("Wash dishes")
            };

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetNextThingToDo_WhenAllExpired_ReturnsNull()
        {
            //Arrange
            var manager = MakeToDoListManager();
            manager.Add(new ToDo("Go out with the dog", new DateTime(2010, 1, 1, 14, 30, 0)));
            manager.Add(new ToDo("Wash dishes", new DateTime(2010, 1, 1, 20, 30, 0)));

            //Act
            SystemTime.Now = () => new DateTime(2010, 1, 1, 21, 0, 0);
            var result = manager.GetNextThingToDo();

            //Assert
            Assert.IsNull(result);
        }
        [Test]
        public void GetNextThingToDo_WhenAllActive_ReturnsNextToDo()
        {
            //Arrange
            var manager = MakeToDoListManager();
            manager.Add(new ToDo("Go out with the dog", new DateTime(2010, 1, 1, 14, 30, 0)));
            manager.Add(new ToDo("Wash dishes", new DateTime(2010, 1, 1, 11, 30, 0)));

            //Act
            SystemTime.Now = () => new DateTime(2010, 1, 1, 10, 0, 0);
            var result = manager.GetNextThingToDo();
            var expected = new ToDo("Wash dishes", new DateTime(2010, 1, 1, 11, 30, 0));

            //Assert
            Assert.AreEqual(expected, result);
        }
        [Test]
        public void GetNextThingToDo_WhenOnlyOneActive_ReturnsNextToDo()
        {
            //Arrange
            var manager = MakeToDoListManager();
            manager.Add(new ToDo("Go out with the dog", new DateTime(2010, 1, 1, 8, 30, 0)));
            manager.Add(new ToDo("Wash dishes", new DateTime(2010, 1, 1, 9, 30, 0)));
            manager.Add(new ToDo("Cook dinner", new DateTime(2010, 1, 1, 11, 30, 0)));

            //Act
            SystemTime.Now = () => new DateTime(2010, 1, 1, 10, 0, 0);
            var result = manager.GetNextThingToDo();
            var expected = new ToDo("Cook dinner", new DateTime(2010, 1, 1, 11, 30, 0));

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetNextThingToDo_WhenTwoToDosWithTheSameName_ReturnsNextToDo()
        {
            //Arrange
            var manager = MakeToDoListManager();
            manager.Add(new ToDo("Go out with the dog", new DateTime(2010, 1, 1, 8, 30, 0)));
            manager.Add(new ToDo("Wash dishes", new DateTime(2010, 1, 1, 9, 30, 0)));
            manager.Add(new ToDo("Cook dinner", new DateTime(2010, 1, 1, 11, 30, 0)));
            manager.Add(new ToDo("Wash dishes", new DateTime(2010, 1, 1, 12, 30, 0)));

            //Act
            SystemTime.Now = () => new DateTime(2010, 1, 1, 12, 0, 0);
            var result = manager.GetNextThingToDo();
            var expected = new ToDo("Wash dishes", new DateTime(2010, 1, 1, 12, 30, 0));

            //Assert
            Assert.AreEqual(expected, result);
        }

    }
}
