# ToryValue v1.7.5

## 익스포트 방법

`ToryValue` 폴더를 익스포트하세요.

- 또는 `ToryValue` 폴더를 작업 중인 프로젝트에 복사해도 됩니다.
- 토리밸류는 토리프레임워크에 의존성이 없습니다.

## 임포트 방법

따로 없습니다. 임포트하면 아래가 자동으로 설정됩니다.

- `SecureKeysManager` 컴포넌트의 `Script execution order`가 `-9999`로 자동 설정됩니다.
- `Scripting define symbol`에 `TORY_VALUE` 심볼이 자동으로 추가됩니다.

## 플레이어프리프스엘리트 사용법

`SecureKeysManager`는 `PlayerPrefsElite` 라이브러리를 사용하기 위한 게임 오브젝트입니다. 이 프리팹이 없이는 토리밸류가 동작하지 않으므로 씬에 반드시 포함해 주세요.

## 토리밸류 사용법

토리밸류는 아래처럼 C# 스크립트와 유니티 에디터를 통해 사용합니다.

```c#
using ToryValue;

class AnyClass
{
    // Non-serialized fields: can be defined manually.
    ToryFloat myFloat;
    ToryInt myInt;
    ToryBool myBool;
    
    // Serialized fields: can be exposed to and managed by the Unity editor.
    [SerializeField] ToryString myStr;
    [SerializeField] ToryVector2 myVec2;
    [SerializeField] ToryVector3 myVec3;
    [SerializeField] ToryVector4 myVec4;
    
    // Enum
    // NOTE: ToryEnum<T> cannot be serialized because it is a generic type.
    ToryEnum<AnyEnumDefinedByYou> myEnum;
    
    void Awake()
    {
        // Defining
        myFloat = new ToryFloat("AnyNameCanBeOK");
        myInt = new ToryInt("My Int", 3); // Defining with the Key and Value. The DefaultValue and SavedValue are set to same as the Value.
        myBool = new ToryBool("My Bool", true, false); // Defining with the Key, Value and DefaultValue. The SavedValue is set to same as the DefaultValue.
        // You can alse define the ToryValue with passing all initial values.
    }
}
```
