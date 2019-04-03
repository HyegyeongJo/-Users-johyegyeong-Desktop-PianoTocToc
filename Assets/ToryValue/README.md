# ToryValue

토리밸류는 값의 저장, 불러오기, 그리고 초기화와 관련된 인터페이스를 제공하는 클래스의 집합입니다. 스크립팅을 통해 토리밸류 형식의 인스턴스를 생성하면 위 기능을 바로 사용할 수 있습니다. 원한다면 유니티 에디터에 노출시켜 에디터를 통해서 조작할 수도 있습니다. 토리엔진 전반에 걸쳐 값의 저장, 불러오기, 초기화에 두루 사용되고 있습니다. 

## 기능

- 스크립팅을 통해 토리밸류 형식의 인스턴스를 생성할 수 있습니다.
- 저장, 불러오기, 초기화 메소드를 제공합니다.
- 위 과정에 보안(앱 외부에서 값의 변경을 감시 및 차단)을 제공합니다.
- `bool`, `int`, `float` ,`string`, `Vector2`, `Vector3`, `Vector4`, `Rect`, `RectInt`,  `enum<T>` 형식을 지원합니다.
- (`enum<T>`을 제외한) 모든 토리밸류 인스턴스는 유니티 인스펙터에 노출되어 조작할 수 있습니다.

## 사용법

1. 우선 `Assets/ToryEngine/ToryValue/Prefabs`폴더의  `SecureKeysManager` 프리팹을 씬에 로드합니다. (이 과정은 `PlayerPrefsElite` 라이브러리를 통해 저장, 불러오기 기능의 보안을 제공하기 위해서 필요합니다.)

2. 스크립트를 생성하고 `ToryEngine` 네임스페이스를 사용합니다.

   ```c#
   using ToryEngine;
   ```

3. 아래의 방법중 하나로 토리밸류를 선언하고 정의할 수 있습니다.

   ```c#
   // 1. 키와 값을 통해 정의합니다.
   ToryBool toryBool = new toryBool("Any key available", false);
   // 2. 키를 통해서만 정의합니다. 값은 해당 형식의 기본값으로 초기화됩니다.
   ToryInt toryInt = new ToryInt("Another key available");
   // 3. 유니티 시리얼라이즈를 이용합니다.
   // 3a. 정의하지 않아도 됩니다.
   ToryFloat toryFloat;
   // 3b. 정의한다면, 정의한 키와 값으로 시리얼라이즈됩니다.
   toryString toryStr = new ToryString("Another key 2", "any value available"); 
   ```

4. 값을 조회하고 할당할 수 있습니다.

   ```c#
   // 1. Value 프로퍼티에 직접 값을 할당합니다.
   toryBool.Value = true;
   // 2. Value 프로퍼티의 값을 조회합니다.
   Debug.Log(toryInt.Value);
   // 2a. 편의를 위해 변수명만으로도 Value 프로퍼티의 값을 조회할 수 있도록 했습니다.
   Debug.Log(toryFloat);
   // NOTE: Value 프로퍼티의 할당은 1 처럼 명시적으로 선언해야만 합니다. 명확한 사용을 위함입니다.
   ```

5. 인스턴스 메소드를 통해 저장, 불러오기, 초기화 기능을 사용할 수 있습니다.

   ```c#
   toryBool.Save(); // 현재 값을 저장
   toryInt.Load(); // 최근에 저장된 값을 불러오기
   toryFloat.Revert();	// 변수를 선언할 때의 초기값으로 초기화
   ```

6. 값이 변경, 저장, 불러오기, 초기화될 때마다 이벤트가 불립니다.

   ```c#
   toryBool.Changed += (bool value) => { Debug.Log("Value changed to " + value); };
   toryInt.Saved += (int value) => { Debug.Log("Value saved to " + value); };
   toryFloat.Loaded += (float value) => { Debug.Log("Value loaded from" + value); };
   toryStr.Reverted += (string value) => { Debug.Log("Value reverted to " + value); };
   ```

7. 토리밸류가 유니티와 시리얼라이즈 되어 있다면 인스펙터를 통해 키, 값, 초기값, 저장값을 확인하고 설정할 수 있습니다. 만약 값이 저장되어 있다면 저장값이 푸른색으로, 그렇지 않다면 붉은색으로 표시됩니다.

8. `Value` 프로터티의 `get/set`을 정의하는 `function`을 설정할 수 있습니다.

1. ```c#
   System.Func<int, int> setterFunc = (value) => { return Mathf.Max(0, value) };
   System.Func<int, int> getterFunc = (value) => { return value; };
   // 1. 초기화시 구조체를 통해 설정할 수 있습니다.
   // 1a. setterFunc만 설정합니다.
   // 아래 예의 경우, Value 프로퍼티는 0보다 큰 값만 할당됩니다.
   ToryInt myIntA = new ToryInt("myIntA", 0, setterFunc);
   // 1b. setterFunc와 getterFunc를 모두 설정합니다.
   ToryInt myIntB = new ToryInt("myIntB", 0, setterFunc, getterFunc);
   // 2. 런타임 중에도 설정할 수 있습니다.
   myIntA.SetterFunc = (value) => { return Mathf.Clamp(value, 0, 10); };
   myIntB.GetterFunc = (value) => { return value; }; // 기본으로 설정된 function입니다.
   ```

## 기타

토리밸류를 임포트하면 아래가 자동으로 설정됩니다.

- `SecureKeysManager` 컴포넌트의 `Script execution order`가 `-9999`로 자동 설정됩니다.
- `Scripting define symbol`에 `TORY_VALUE` 심볼이 자동으로 추가됩니다.