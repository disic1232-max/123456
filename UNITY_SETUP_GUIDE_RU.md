# Unity 6: как импортировать проект ChemLab и всё настроить (подробно, для новичка)

Ниже инструкция **шаг за шагом**: от пустого проекта до рабочей лаборатории, где можно подбирать предметы, класть их, смешивать вещества и запускать реакции.

---

## 0) Что нужно установить заранее

1. **Unity Hub** (последняя версия).
2. **Unity 6 Editor** (например, 6000.x LTS/Tech Stream — используй ту же ветку, что в твоей команде).
3. Модуль под платформу сборки (Windows/Mac/Linux).
4. IDE для C#:
   - Visual Studio / Rider / VS Code + C# extension.

---

## 1) Создание нового проекта

1. Открой **Unity Hub**.
2. Нажми **New project**.
3. Шаблон: **3D (URP)** (рекомендовано для красивого света и VFX) или просто **3D Core**.
4. Название проекта: `ChemLab`.
5. Нажми **Create project**.

> Почему URP: проще добиться чистой современной картинки для минималистичного UI и лабораторных эффектов.

---

## 2) Импорт твоих файлов в Unity

### Вариант A (самый простой)
1. Закрой Unity Editor (желательно).
2. Скопируй папку `Assets/ChemLab` из репозитория в папку `Assets` твоего Unity проекта.
3. Скопируй `README_RU.md` и `UNITY_SETUP_GUIDE_RU.md` в корень проекта (не обязательно, но удобно).
4. Открой проект снова в Unity Hub.

### Вариант B (через git)
1. Открой терминал в папке проекта.
2. Подтяни изменения репозитория.
3. Убедись, что папка `Assets/ChemLab/Scripts/...` появилась.

Unity автоматически перекомпилирует C# скрипты.

---

## 3) Проверка зависимостей в Package Manager

Открой **Window → Package Manager** и проверь:

1. **Input System** (если хочешь перейти на новый ввод; текущий код работает и со старым Input Manager).
2. **TextMeshPro** (нужен для `TMP_Text` в `ChemistryCodexUI`).
   - Если TMP не инициализирован, Unity предложит **Import TMP Essentials** — нажми.
3. (Опционально) **Cinemachine** для удобной камеры от первого лица.

---

## 4) Подготовка структуры сцены

Создай новую сцену `MainLab` и сохрани в `Assets/Scenes/MainLab.unity`.

В Hierarchy создай:

1. `GameSystems` (Empty GameObject)
2. `Player` (Capsule)
3. `Main Camera` (внутрь Player)
4. `LabRoom` (пол/стены)
5. `UI` (Canvas)

---

## 5) Настройка игрока (FPS)

### 5.1 Player
1. На `Player` добавь:
   - `CharacterController`
   - (любой твой FPS movement скрипт, если есть)
   - `PlayerInventory` (из `Assets/ChemLab/Scripts/Inventory/PlayerInventory.cs`)
   - `PlayerInteractor` (из `Assets/ChemLab/Scripts/Core/PlayerInteractor.cs`)
2. Укажи в `PlayerInteractor`:
   - `Player Camera` = `Main Camera`
   - `Inventory` = компонент `PlayerInventory` на Player
   - `Interact Distance` = 3
   - `Interact Mask` = слой `Interactable` (создай ниже)

### 5.2 Слой Interactable
1. **Edit → Project Settings → Tags and Layers**.
2. Добавь слой: `Interactable`.
3. Назначай этот слой всем подбираемым предметам и поверхностям, куда можно ставить.

---

## 6) Настройка UI (Tab: кодекс)

1. Создай `Canvas` (Screen Space - Overlay).
2. Создай внутри панель `CodexPanel`.
3. Добавь 3 текста TextMeshPro:
   - `ElementsText`
   - `ReactionsText`
   - `HintsText`
4. На объект `UI` добавь скрипт `ChemistryCodexUI`.
5. Свяжи поля:
   - `Root` = `CodexPanel`
   - `Elements Text` = `ElementsText`
   - `Reactions Text` = `ReactionsText`
   - `Hints Text` = `HintsText`
6. Отключи `CodexPanel` в инспекторе (чтобы открывался по Tab).

---

## 7) Создание химических данных (ScriptableObject)

### 7.1 Папки под данные
Создай папки:
- `Assets/ChemLab/Data/Elements`
- `Assets/ChemLab/Data/Reactions`
- `Assets/ChemLab/Prefabs`

### 7.2 Создание элементов
1. ПКМ в `Assets/ChemLab/Data/Elements` → **Create → ChemLab → Chemistry → Element**.
2. Создай, например:
   - `Element_H2`
   - `Element_O2`
   - `Element_H2O`
3. Для каждого заполни:
   - `elementId` (уникальный, например `h2`)
   - `displayName` (Hydrogen)
   - `formula` (H2)
   - `defaultState`, `meltingPointC`, `boilingPointC`, `reactivity`, `ph`, `radioactive`, `baseColor`

### 7.3 Создание реакции
1. ПКМ в `Assets/ChemLab/Data/Reactions` → **Create → ChemLab → Chemistry → Reaction**.
2. Пример: `Reaction_H2_Combustion`.
3. Настрой:
   - `reactants`: H2 (2), O2 (1)
   - `products`: H2O (2)
   - `conditions.temperatureRangeC`: например (200, 4000)
   - `conditions.requiredCatalyst`: null (или платина, если хочешь)
   - `conditions.minPressureAtm/maxPressureAtm`: (0.8, 2.0)
   - `canExplode`: true
   - `canReleaseToxicGas`: false
   - VFX-флаги: по желанию

---

## 8) Создание контейнера (пробирка/колба)

1. Создай 3D объект (например Cylinder) и назови `TestTube`.
2. Добавь компонент `ChemicalContainer`.
3. В `knownReactions` добавь созданные Reaction SO.
4. Для визуала жидкости:
   - Внутри пробирки создай дочерний объект `Liquid` (Cylinder/Quad/Mesh).
   - Назначь его Renderer в поле `liquidRenderer`.
5. Создай и привяжи ParticleSystem:
   - `smokeFx`
   - `bubblesFx`
   - `precipitateFx`

Сделай prefab: перетащи `TestTube` в `Assets/ChemLab/Prefabs`.

---

## 9) Настройка инвентарных предметов

### 9.1 Item SO
1. ПКМ в `Assets/ChemLab/Data` → **Create → ChemLab → Inventory → Item**.
2. Создай `Item_TestTube`.
3. Заполни:
   - `itemId`: `test_tube`
   - `displayName`: Test Tube
   - `worldPrefab`: prefab `TestTube`
   - `stackable`: false (обычно для контейнеров)

### 9.2 World item
1. Создай объект `World_TestTubeItem` в сцене.
2. Добавь Collider + Rigidbody (если нужен физический подбор).
3. Добавь `WorldInventoryItem`:
   - `ItemData` = `Item_TestTube`
   - `Amount` = 1
4. Слой = `Interactable`.

Теперь ПКМ должен подбирать, ЛКМ — ставить выбранный предмет.

---

## 10) Оборудование лаборатории

Создай объекты и повесь скрипты:

1. `Burner` → `BurnerEquipment`
2. `Cooler` → `CoolerEquipment`
3. `Centrifuge` → `CentrifugeEquipment`
4. `Filter` → `FilterEquipment` (пока заготовка)
5. `Electrolyzer` → `ElectrolyzerEquipment`

Как использовать логически:
- Когда контейнер “вставлен” в оборудование, вызывай `equipment.Process(container, Time.deltaTime)` каждый кадр.
- Это можно сделать отдельным контроллером `EquipmentSlotController` (добавишь позже).

---

## 11) Как быстро проверить, что всё работает

1. Нажми Play.
2. Подбери предмет ПКМ.
3. Выбери слот 1..6.
4. Поставь предмет ЛКМ.
5. Нажми Tab — должен открыться Codex.
6. Через debug-кнопки/временный скрипт добавь вещества в `ChemicalContainer.AddSubstance(...)`.
7. Подними температуру (`SetTemperature`) и проверь запуск реакции + VFX.

---

## 12) Частые ошибки новичков (и как исправить)

1. **NullReferenceException в PlayerInteractor**
   - Не назначена камера или inventory в инспекторе.
2. **Tab не открывает UI**
   - Не привязан `root`/TMP поля в `ChemistryCodexUI`.
3. **Не подбираются предметы**
   - Неверный `InteractMask` или нет `WorldInventoryItem` на объекте.
4. **Реакции не запускаются**
   - Контейнер не содержит нужных реагентов/пропорций/температуры/давления.
5. **Нет текста TMP**
   - Не импортированы TMP Essentials.

---

## 13) Рекомендованный следующий шаг (чтобы довести до играбельного прототипа)

1. Сделать `EquipmentSlotController` (контейнер вставляется в оборудование).
2. Добавить `RecipeDebugPanel` (кнопки Add H2, Add O2, Heat +100C).
3. Реализовать фильтрацию осадка в `FilterEquipment`.
4. Добавить сохранение открытий (`DiscoverySystem`) через JSON.
5. Добавить 10 реальных школьных реакций (нейтрализация, осаждение, окисление и т.д.).

---

## 14) Мини-чеклист перед коммитом

- [ ] Все ссылки в инспекторе проставлены.
- [ ] Нет Missing Script в сцене/префабах.
- [ ] Вещества и реакции созданы как SO.
- [ ] Хотбар 1–6 переключается.
- [ ] Tab открывает/закрывает кодекс.
- [ ] Минимум 1 реакция проходит полный цикл визуально.

Удачи! Если хочешь, следующим шагом я могу сразу добавить готовые скрипты:
- `EquipmentSlotController`
- `RecipeDebugPanel`
- `Quest_CreateSubstanceX`
- и набор из 10 стартовых реалистичных реакций (с параметрами прямо в коде/JSON).
