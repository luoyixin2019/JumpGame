# JumpGame
Retro the jump game at space time~

## 创新
在设计过程中发现：如果每一次跳到新的台阶上，总是新实例化一个新的下一个台子的GameObject，会导致资源的浪费，尤其是到游戏后期，前面太多跳过的台子会占用不少的资源。
因此，针对这个问题，设计了跳台的对象池，设定了台子数量的上限，当超过这个阈值时，会回收最早的一个台子的GameObject对象，将其作为下一个跳台，起到了重复利用，节约资源开销的作用。
（通过最后两张图可以看到，新生成的青色跳台是通过回收先前的那个紫色的跳台而完成的）

## TODO
1.游戏难度随着游戏的进行而增加；  
2.声音的加入，动画的改进；  
3.游戏结束后分数结算场景。  

## 部分截图
开始界面：
![图片](https://user-images.githubusercontent.com/49396177/113801093-bac9cb00-978a-11eb-9f78-23f4cc552f65.png)

游戏截图：
![图片](https://user-images.githubusercontent.com/49396177/113801108-c0bfac00-978a-11eb-9414-8e1f8211042c.png)

![图片](https://user-images.githubusercontent.com/49396177/113801158-da60f380-978a-11eb-84bd-516628673df1.png)

![图片](https://user-images.githubusercontent.com/49396177/113801465-7854be00-978b-11eb-8345-36fde7237cdc.png)

![图片](https://user-images.githubusercontent.com/49396177/113801485-8276bc80-978b-11eb-9620-d908c2319731.png)
