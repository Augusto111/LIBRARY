# Book Management System 图书管理系统

本项目为北京邮电大学数据结构课程设计中图书管理系统项目

项目均已实现课程设计要求的全部功能，并通过测试

# Function 功能需求说明

- 每种图书包括图书编号、书名、作者、出版社、简介、购入时间、数量等信息
- 图书馆在购书时可根据图书的信息自动生成编码
- 教师、学生包括姓名、工作证（学生证）号、所在学院、借书权限、借书数量等信息
- 借书时填写借书单，还书时填写借书单还书项，并记录入日志文件，借书超期要罚款
- 基本功能有：建立图书基本信息文件、读者基本信息文件、图书入库（编码）、借书（预约）、还书、查询等
- 查询、统计功能有：
  - 读者可根据图书的名称、编码、作者、出版社等信息查询图书的其他信息；也可根据图书的分类等信息查询图书的具体信息等，包括某图书库中尚存多少本
  - 查询教师、学生已借出的图书情况，读者的借书诚信情况（是否有过超期的情况）等
  - 图书的借阅率等
- 对于库中已借完的图书，读者可预约，待库中有还回时能提示通知该读者
- 模拟系统以时间为轴向前推移，每10秒左右向前推进 一天（有键盘或鼠标操作时不计时）
- 建立日志文件，对图书借阅情况进行记录
- 不得使用数据库

# Getting Started 使用手册

- 项目运行平台：`windows 7/8/8.1/10` （`xp`及以下版本未测试）
- 项目开发平台：`Visual Studio 2015`
- 解决方案包含两个项目
  1. `Library` 图书管理系统前端项目 采用`Winform`开发，部分控件基于第三方库DMSkin
  2. `LibrarySystemBackEnd` 图书管理系统后端项目
- 项目初始管理员账号 `admin` 密码 `admin` 使用者可以用此账号向书库添加图书
- 用户注册开放，暂无相关注册信息合法性验证，只要学号、账户名不重复即可注册成功
- 有关老师与学生用户类别，教师可以比学生借阅更多的书籍
- 系统时间根据题目要求使用模拟时间，5秒未操作程序，日期推后1日



# Tests 运行实例

![1](https://github.com/Kou-Akira/LIBRARY/blob/master/LIBRARY/DescribeImage/1.png)

![2](https://github.com/Kou-Akira/LIBRARY/blob/master/LIBRARY/DescribeImage/2.png)

![3](https://github.com/Kou-Akira/LIBRARY/blob/master/LIBRARY/DescribeImage/3.png)

# Contributors 参与者

- **Xiaoyu Huang**  **黄晓宇** - *Front-end Build* - [Kou-Akira](https://github.com/Kou-Akira)


- **Zhixuan Xiao** **肖智轩** - *Back-end Build* - [xzxxzx401](https://github.com/xzxxzx401)
- **Shengpeng Luo 罗圣鹏** - *Test work* 

