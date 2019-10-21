# 命令行配置

    从程序入口中读取参数并打印。

# 注意事项

    MicroSoft.AspNetCore.APP(ASP.NET Core共享框架包)，包括一个典型的应用程序所需要的所有东西，目前拥有150个明确列出的依赖项，7个月前是144项。其中有9个不同的认证提供程序包：
      
       Cookies
       FaceBook
       Google
       JwtBearer
       Microsoft Account
       OAuth
       OpenIdConnect
       Twitter
       WsDederation

    从这150个依赖项列表中，31个将从ASP.NET Core 3.0的共享框架中删除，因为他们违反了新的纳入标准：

        (1) 依赖我们无法提供服务的第三方代码
        (2) 组件本身在3.0中被弃用
        (3) 它们实施的协议或身份验证机制极易发生变化

    如果我们只考虑一个简单的REST服务器，只需要直接引用这三个包：

        Microsoft.AspNetCore
        Microsoft.AspNetCore.Mvc
        Microsoft.AspNetCore.HttpsPolicy
    