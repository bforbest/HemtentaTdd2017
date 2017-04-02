using Hemtenta_Alisina_Housela.blog;
using Moq;
using System;
using Xunit;
namespace Hemtenta_Alisina_Housela_Test
{
    
    public class BlogTest
    {
        Blog blog;
        User user;
        private void LoggInUser()
        {
            var mock = new Mock<IAuthenticator>();
            user = new User("username");
            blog = new Blog();
            mock.Setup(x => x.GetUserFromDatabase(user.Name)).Returns(new User(user.Name));
            blog.Authenticator = mock.Object;
            blog.LoginUser(user);
        }
        [Fact]
        public void UserLoggedInSuccess()
        {
            LoggInUser();
            Assert.True(blog.UserIsLoggedIn);
        }
        [Fact]
        public void UserLoggedInFailer_Throws_InvalidUserException()
        {
            var blog = new Blog();
            Assert.Throws<UserNullException>(()=>blog.LoginUser(null));
        }
        [Fact]
        public void LogOutUser_Success_ReturnFalse()
        {
            LoggInUser();
            blog.LogoutUser(user);
            Assert.False(blog.UserIsLoggedIn);
        }
        [Fact]
        public void LogOutUser_Failer_Throws_UserNullException()
        {
            LoggInUser();
            Assert.Throws<UserNullException>(() => blog.LogoutUser(null));
        }
        [Theory]
        [InlineData(null,"")]
        [InlineData("", null)]

        public void PageNotValid_Throws_PageExecption(string title, string content)
        {
            LoggInUser();
            var page = new Page { Title = title, Content = content };
            Assert.Throws<PageException>(() => blog.PublishPage(page));
        }
        [Fact]
        public void PagePublishedSucceed_ReturnTrue()
        {
            LoggInUser();
            var page = new Page { Title = "title", Content = "content" };
            Assert.True(blog.PublishPage(page));
        }
        [Fact]
        public void SendMailSucceed()
        {
            LoggInUser();
            var result =blog.SendEmail("@", "caption", "body");
            Assert.Equal(1, result);
        }
        [Theory]
        [InlineData(null, null, null)]
        [InlineData("", "", "")]
        public void SendMailFailer_ParameterNullOrEmpty(string address, string caption, string body)
        {
            LoggInUser();
            var result = blog.SendEmail(address, caption, body);
            Assert.Equal(0, result);
        }
        [Fact]
        public void SendMailFailer_UserNotLoggedIn()
        {
            var blog = new Blog();
            var result = blog.SendEmail("@", "caption", "body");
            Assert.Equal(0, result);
        }
    }
}
