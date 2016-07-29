using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDataBase {
    /*
     
     static void Main(string[] args)
        {
            //这个程序演示了XmlDatabase的简单使用方式.注意，目前的版本是alpha，后续可能还会有所更改


            //第一步：创建或者打开一个数据库
            using (XDatabase db = XDatabase.Open("Test"))
            {
                //如果要重定向日志输出，请使用下面的语法即可
                db.Log = Console.Out;


                #region //第二步：创建几个数据实体类型.
                //值得说明一下，这些数据实体类型没有任何特殊的要求，不需要添加任何的Attribute。但最好是遵守以下的两条简单的规则
                //1.属性都是可读可写的，都是Public的
                //2.重写ToString方法，这是为了让引擎在做日志记录的时候用的

                Customer customer = new Customer()
                {
                    CustomerID = "ALFKI",
                    CompanyName = "Alfreds Futterkiste",
                    Country = "Germany",
                    Region = "BC",
                    City = "Berlin",
                    ContactName = "Maria Anders",
                    Address = "Obere Str. 57"
                };

                Employee employee = new Employee()
                {
                    EmployeeId = 1,
                    FirstName = "Nancy",
                    LastName = "Davolio",
                    Title = "Sales Representative"
                };


                Product product = new Product()
                {
                    ProductId = 1,
                    ProductName = "Chai"
                };
                
                Order Order = new Order()
                {
                    OrderID = 10248,
                    OrderDate = DateTime.Now,
                    Customer = customer,
                    Employee = employee,
                    OrderItems = new List<OrderItem>()
                    {
                        new OrderItem(){
                            Product=product,
                            UnitPrice=20.5m,
                            Quantity=3
                        },
                        new OrderItem(){
                            Product=new Product(){ 
                                ProductId=2, 
                                ProductName="Grandma's Boysenberry Spread"},
                            UnitPrice=333,
                            Quantity=1
                        }
                    }
                };

                #endregion


                //第三步：插入对象
                db.Store(Order);//只要调用该方法就可以了。默认情况下，这是立即生效的
                

                
                //第四步：读取对象 （这里还可以做任何的LINQ查询操作）
                var query = from o in db.Query<Order>()
                            where o.OrderID==10248
                            select o;
                Order firstOrder = query.First();
                Console.WriteLine("{0},总金额为:{1}", firstOrder, firstOrder.OrderItems.Sum(i => i.Quantity * i.UnitPrice));


                //第五步：更新对象
                firstOrder.OrderID = 10249;//假设我们修改一下订单编号
                //你还可以对该对象做任何的修改，例如增加一个订单记录
                firstOrder.OrderItems.Add(new OrderItem()
                {
                    Product = new Product() { ProductId = 3, ProductName = "Mishi Kobe Niku" },
                    UnitPrice = 10000,
                    Quantity = 1
                });

                db.Store(firstOrder);//更新的语法与刚才插入的时候是一模一样的，内部会判断出来到底是更新还是新增
                Console.WriteLine("{0},总金额为:{1}", firstOrder, firstOrder.OrderItems.Sum(i => i.Quantity * i.UnitPrice));


                //第六步：删除对象
                db.Delete(firstOrder);

                //第七步：批处理（上面的操作都是立即生效的，如果操作数不多的话，很简单也很直接，但因为每次都涉及到数据文件的读写，如果我们有一个循环，要批量做一些事情，则可能会有性能方面的问题，所以下面提供了批处理模式）

                //首先得将数据库的模式切换到批处理模式
                db.AutoSubmitMode = false;
                for (int i = 0; i < 10; i++)
                {
                    Order temp = new Order()
                    {
                        OrderID = 10248,
                        OrderDate = DateTime.Now,
                        Customer = customer,
                        Employee = employee,
                        OrderItems = new List<OrderItem>()
                    {
                        new OrderItem(){
                            Product=product,
                            UnitPrice=20.5m,
                            Quantity=3
                        },
                        new OrderItem(){
                            Product=new Product(){ 
                                ProductId=2, 
                                ProductName="Grandma's Boysenberry Spread"},
                            UnitPrice=333,
                            Quantity=1
                        }
                    }
                    };

                    db.Store(temp);//语法还是一模一样的
                }

                //区别在于，如果是批处理模式，则必须明确地调用SubmitChanges才生效
                XSubmitStatus status = db.SubmitChanges();
                //这个SubmitChanges方法默认情况下，如果遇到某个操作出错，那么后面的操作就不会进行了。
                //如果希望出错后继续，那么应该使用下面的语法
                //XSubmitStatus status=db.SubmitChanges(true);

                //第八步：事务性操作
                //要进行事务性操作，必须处于批处理模式中。我最后将它做了简化，调用下面的方法即可
                //XSubmitStatus status = db.SubmitChangesWithTransaction();


                //第九步：关闭数据库
                db.Close();
            }

            Console.Read();
            //后续任务
        }
     
     */
    public class demo {

    }
}
