# SAXS

**SAXS** — это веб-приложение для создания, визуализации и анализа 3D-систем фуллеренов и других частиц. Оно разработано с использованием ASP.NET Core, React и PostgreSQL.

## 🚀 Возможности

### 🔐 Аутентификация

* Встроенная система регистрации и входа пользователей (ASP.NET Core Identity).
* Поддержка подтверждения email.
* Возможность расширения внешними поставщиками (Google, Facebook и др.).

<img width="1271" height="614" alt="image" src="https://github.com/user-attachments/assets/f8642ff1-2682-48b5-8953-46ef36efeb9c" />

---

### ⚙️ Создание систем

* Выбор типа частицы (например, C60, икосаэдр и др.).
* Настройка параметров: размер, вращение, форма, масштаб и количество частиц.
* Поддержка двух вариантов конфигурации: через радиус области или через количество частиц.

<img width="1262" height="1302" alt="image" src="https://github.com/user-attachments/assets/e3d9461f-7350-4bee-a0aa-ba4745160add" />

---

### 🧪 Визуализация и анализ

* Отображение 3D-системы частиц с помощью интерактивной WebGL-визуализации (Three.js).
* Генерация графиков плотности распределения.
* Вывод расчетных значений (`phi`, `qMin`, `qMax`) и экспорт данных для Mathcad.

<img width="1259" height="1300" alt="image" src="https://github.com/user-attachments/assets/39d610d0-57da-4c26-8c0b-6cf9187d94bb" />

---

## 🛠️ Технологии

* **Backend:** ASP.NET Core Web API
* **Frontend:** React + TypeScript + Three.js
* **База данных:** PostgreSQL
* **Аутентификация:** ASP.NET Identity
* **Обработка графики:** WebGL (через Three.js)
* **Хранение систем и параметров:** Entity Framework Core

---

## 📦 Локальный запуск

   ```bash
   dotnet build
   dotnet ef database update --project Fullstack.SAXS.Infrastructure --startup-project Fullstack.SAXS.Server
   cd Fullstack.SAXS.Server
   dotnet run
   ```

---

## 📂 Структура проекта

```
/Fullerenes.Server              - ASP.NET Core Web API и Identity
/fullstack.saxs.client          - Клиентская часть на React
/Fullstack.SAXS.Domain          - Сущности предметной области (DDD)
/Fullstack.SAXS.Application     - Сервисный слой (use-cases, интерфейсы)
/Fullstack.SAXS.Infrastructure  - Инфраструктурные зависимости (например, работа с файлами)
/Fullstack.SAXS.Persistence     - Доступ к данным (PostgreSQL, EF Core)
```

---

## 🧑‍🔬 Применение

Идеально подходит для:

* Исследования 3D-структур частиц
* Генерации входных данных для Mathcad и других научных пакетов
* Научных публикаций и визуализации результатов

---

## 📬 Обратная связь

По вопросам и предложениям: [denisyakubov321@gmail.com](mailto:denisyakubov321@gmail.com)

---
