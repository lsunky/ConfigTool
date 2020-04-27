# ConfigTool
配置表生成，读取，解析工具。editor和运行时分开，支持脚本调用。
## Editor 编辑器环境
### IConfigGenerator 配置文件构建器
配置文件构建器，配置文件的底层接口，不同类型的生成器，都要实现此接口。

### ClassGenerator 自动代码生成器

可以只根据配置excel，自动生成对应解析代码和mgr代码。

### Excel2ListUtil 
excel解析帮助类，把配置excel读取到内存，并用转化为工具内的List<SheetData>。如果不想用npoi解析excel,则只需要修改对应解析库和Excel2ListUtil文件即可，不影响外部逻辑。

#### ByteConfigGenerator

二进制格式的配置生成器，把excel转换为对应的二进制文件。

### JsonConfigGenerator

json格式的配置生成器，把excel转换为对应的json类。


## Runtime 运行时

### IConfigUnSerialize

配置反序列化接口。
### ByteConfigUnSerialize

二进制反序列化规则，此规则要跟**ByteConfigGenerator**严格对应

### JsonConfigUnSerialize

json反序列化规则，此规则跟**JsonConfigGenerator**严格对应


