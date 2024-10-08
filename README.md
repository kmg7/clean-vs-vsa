# VSA ve Clean Architecture 

Clean archicecture ve Vertical Slice Architecture mimarilerinin kıyaslandığı bir takım çalışması.

### Şunlar deneyimlendi
- CQRS pattern
- Mediator pattern
- Repository pattern
- Result pattern
- Postgres JSONB dotnet entegrasyonu
- EntityFramework
- FluentValidation
- ProblemDetails RFC-7807
- xUnit
- FluentAssertions
- Moq
- Bogus
- NSubstitute

### Klasör Yapıları
#### Clean Architecture
```
src/
├── core/
│   ├── Application/
│   │   ├── Behaviours/
│   │   ├── Features/
│   │   │   ├── Presentations/
│   │   │   │   ├── Commands/
│   │   │   │   ├── Queries/
│   │   │   │   └── IPresentationsRepository.cs
│   │   │   ├── Slides/
│   │   │   │   └── ...
│   │   │   └── Users/
│   │   │       └── ...
│   │   └── Shared/
│   └── Domain/
│       ├── Common/
│       │   ├── Enums/
│       │   ├── Errors/
│       │   └── Exceptions/
│       ├── Entities/
│       └── Extensions/
├── infrastructure/
│   ├── Persistence/
│   │   ├── Databases/
│   │   └── Migrations/
│   └── Infrastructure/
│       └── Services/
│           └── ...        
└── presentation/
    └── Api/
        ├── Endpoints/
        ├── Extensions/
        ├── Filters/
        ├── Requests/
        └── Validators/
tests/
├── core/
│   └── Application.Tests
├── infrastructure/
│   ├── Persistence.Tests
│   └── Infrastructure.Tests
└── presentation/
    └── Api.Tests
```

#### Vertical Slice Architecture
```
src/
└── Api/
    ├── Features/
    │   ├── Orders/
    │   │   ├── Commands/
    │   │   ├── Queries/
    │   │   ├── Constants/
    │   │   ├── Persistence/
    │   │   ├── Domain/
    │   │   │   ├── Entities/
    │   │   │   └── Enums/
    │   │   └── IOrderRepository.cs
    │   ├── Users/
    │   │   └── ...
    │   └── Products/
    │       └── ...
    ├── Filters/
    ├── Migrations/
    └── Shared/
        ├── Behaviors/
        │   ├── Authorization/
        │   ├── Validation/
        │   ├── Transaction/
        │   └── ...
        ├── Caching/
        ├── Exceptions/
        ├── Middlewares/
        ├── Security/
        ├── Persistence/
        │   ├── Ef/
        │   └── Mongo/
        └── Entities/
tests/
└── Features/
    ├── Orders/
    │   └── Commands/
    │       └── CreateOrderCommandHandlerTests.cs
    ├── Users/
    │   └── ...
    └── Products/
        └── ...
```

![PackageByImages](assets/package-layer-feature.png)



### VSA ve Clean Arch. mimarilerinin genel karşılaştırılmaları
| **Özellik**            | **Vertical Slice Architecture**                                                                                    | **Clean Architecture**                                                                                     |
|------------------------|-------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------|
| **Tanım**               | Uygulama, bağımsız işlevsel dilimlere ayrılır.                                                                     | Katmanlı yapı; iş mantığı merkezi bir katmanda izole edilir ve dış bağımlılıkları minimumda tutar.           |
| **Yapı**                | Her dilim tüm katmanları kapsar (UI, iş mantığı, veri erişimi).                                                     | İş mantığı iç katmanda izole edilir, dış katmanlar (UI, veri erişimi) buna bağımlıdır.                       |
| **Modülerlik**          | Dilimler arasında bağımsızlık sağlanır.                                                                            | İş mantığı dış katmanlardan bağımsızdır.                                                   |
| **Test Edilebilirlik**  | Dilimler bağımsız test edilebilir.                                                                                 | İş mantığı izole olduğu için test edilmesi kolaydır.                                                         |
| **Karmaşıklık**         | Küçük projelerde daha basit olabilir, ancak büyük projelerde yönetim zorlaşabilir.                                | Başlangıçta karmaşık olabilir, ancak büyük projelerde düzen sağlar.                                          |
| **Kullanım Alanı**      | Küçük ve orta ölçekli projelerde kullanılır. Mikroservis mimarilerine uygun olabilir.                             | Büyük ve karmaşık projelerde kullanılır.                                                                     |
| **Geliştirme Hızı**     | Yeni dilimler hızlı eklenir.                                                                                        | İlk başta yavaş olabilir, ancak sürdürülebilirlik sağlandıktan sonra hızlanır.                               |

#### Veri tabanı için kullanılabilecek konteyner

```
docker run --name pg-json-lab -e POSTGRES_USER=demouser -e POSTGRES_PASSWORD=demopassword -e POSTGRES_DB=demodb -p 5432:5432 -d postgres:16.4

```

#### Ayrıca
> JSONB ile ilgili kaynaklara [resmi dökümandan](https://www.postgresql.org/docs/current/datatype-json.html) ulaşabilirsiniz.

Bu çalışmayı yaparken npgsql entity framework kütüphanesi kullanıldı. Npgsql jsonb tipindeki verileri System.Text.Json apilerini kullanarak işlerken utf8 binary işleme yaptığı için oldukça performanslı olduğunu iddia etmekte.

Örnekler için çalışmanın yoğunlaşıldığı slides uç noktasına bakarken open api dokümanından faydalanabilirsiniz.