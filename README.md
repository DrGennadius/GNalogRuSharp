# GNalogRuSharp
Пример библиотеки получения данных с ФНС.

На текущий момент представлены следующие операции:
1. Получить ИНН по паспортным данным человека. Как если бы использовать https://service.nalog.ru/inn.html
2. Проверка статуса налогоплательщика налога на профессиональный доход (самозанятого).

## Получить ИНН по паспортным данным человека.
```cs
var data = new InnData()
{
    Surname = "Фамилия",
    Name = "Имя",
    Patronymic = "Отчество",
    BirthDate = new DateTime(1990, 1, 1), 
    DocType = DocumentType.PassportRussia, 
    DocNumber = "94 14 435125"
};
var client = new InnService();
var result = await client.GetInnAsync(data);
int code = result.Code;
string inn = result.Inn;
```
```cs
var client = new InnService();
var result = await client.GetInnAsync("Фамилия", "Имя", "Отчество", new DateTime(1990, 1, 1), DocumentType.PassportRussia, "94 14 435125");
int code = result.Code;
string inn = result.Inn;
```
## Проверка статуса налогоплательщика налога на профессиональный доход (самозанятого).
```cs
var client = new TaxpayerStatusService();
var result = await client.GetStatusAsync("123456789");
bool status = result.Status;
string message = result.Message;
```
Можно указать дату, для которой будет осуществлена проверка статуса самозанятого.
```cs
var result = await client.GetStatusAsync("123456789", new DateTime(2021, 1, 1));
```
```cs
var data = new TaxpayerStatusData()
{
    Inn = "123456789",
    RequestDate = "2021-01-01"
};
var result = await client.GetStatusAsync(data);
```

## Пример простого GUI для библиотеки
Каких-то GUI не планировал, но вот что-то немного набросал по быстрому, скриншоты:
![image](https://user-images.githubusercontent.com/27915885/135748525-c45c0ccd-93d6-4859-ad73-da2d1440eb11.png)
![image](https://user-images.githubusercontent.com/27915885/135748542-e325055f-99f9-401c-ad27-f6131c63ca63.png)
