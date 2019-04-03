## Version 1.7.6

#### 버그 수정

- `ToryScene`에서 `TimeSinceStarted`가 음수로 나오던 버그가 해결되었습니다.
- `ToryValue`에서 `SavedValueChanged` 이벤트가 `ValueSaved` 이벤트 이후(`PlayerPrefs`에 저장된 이후)에 불리도록 수정되었습니다.

## Version 1.7.5

#### 버그 수정

- `PlayerPrefs Elite`를 최신 버전으로 업데이트하며, 경로에 의존적인 코드로 인한 에러를 수정하였습니다. 개발자의 개선 의도가 없어보여 직접 수정하는 방법 밖에 없습니다.

#### 라이브러리 업데이트

- `PlayerPrefs Elite`를 v1.5.1로 업데이트하였습니다. 사실 바뀐 건 없는 듯 합니다.

## Version 1.7.4

#### 버그 수정

- `SecureKeysManager.cs` 스크립트가 에티터에서만 사용될 수 있도록 `Editor` 폴더로 이동되었습니다.

## Version 1.7.3

#### 버그 수정

- 현재 토리 씬이 다음 토리 씬으로 넘어갈 때 `Ended` 이벤트가 여러 번 불리 수도 있는 문제를 해결하였습니다.

## Version 1.7.2

#### 버그 수정

- 토리 씬의 `ForcedStayTimeSinceStarted`를 0으로 했을 때, 해당 토리 씬이 다음 토리 씬으로 넘어가지 않던 문제를 해결했습니다.

## Version 1.7.1

#### 버그 수정

- 유니티 클라우드 버그(?)로 싱크가 안되었던 코드를 수동으로 찾아서 마저 싱크했습니다. 그러나 버그 없이 기능만 싱크가 안된 부분도 있을 수 있기에, 너무 불안하여 빗버킷(깃)을 쓰기로 하였습니다. 빗버킷에 유저를 추가하려면 돈이 들어서, 일단은 정정만이 유니티 클라우드(협업용) + 빗버킷(백업용)으로 관리하고 있을게요, 빗버킷에 접근이 필요하신 분들은 정정만에게 말씀주세요.

## Version 1.7.0

#### 피쳐 업데이트

- 토리밸류가 토리프레임워크로부터 분리되었습니다.

  - 프로젝트가 로드될 때, `TORY_VALUE` 스크립팅 디파인 심볼이 없다면 생성합니다.
  - 이제 토리밸류는 `ToryValue` 네임스페이스를 사용합니다.
  - 토리밸류 기능이 업데이트됨에 따라, 그 기능을 쓰는 `ToryInput` 에디터 스크립트가 수정되었습니다. 에디터 기능의 변화는 없습니다.

- 생성자를 더 편하게 사용할 수 있습니다.

  - `Key`와 `Value`만으로 인스턴스를 생성할 때, `DefalutValue`와 `SavedValue`가 `Value` 값으로 자동 설정됩니다.
  - `Key`와 `Value`, `DefaultValue`만으로 인스턴스를 생성할 때, `SavedValue`가 `DefaultValue` 값으로 자동 설정됩니다. 

- 플레이 중에 토리밸류의 값을 에디터에서 변경하면 변경하는 값에 따라  `ValueChanged`, `DefaultValueChanged`, `SavedValueChanged`, `ValueSaved` 이벤트가 불립니다. 원래는 스크립트를 통해서 값을 변경했을 때만 이러한 이벤트가 불렸지만, 에디터 조작에도 이벤트가 불리도록 기능을 추가한 것입니다.

## Version 1.6.1

#### 버그 수정

- 이전 업데이트에서 `TorySceneFlow (Example)` 애니메이터와 `ToryFramework` 프리팹의 기본값 및 설정이 깨져있던 걸 복구하였습니다.

## Version 1.6.0

#### 피쳐 업데이트

- 라이브러이 업데이트 시, 커스텀화한 스크립트를 보호하기 위해 내부 폴더의 트리 구조를 변경하였습니다.
- 위와 관련, `README` 파일을 업뎃하였습니다.

## Version 1.5.0

#### 피쳐 업데이트

- 다른 라이브러리와 클래스 명이 겹치는 충돌을 막기 위하여 심볼을 관리하는 클래스명을 `ToryFrameworkScriptingDefineSymbolGenerator`로 변경하였습니다.
- 토리 씬의 편의를 개선하는 다음 항목들을 추가하였습니다.
  - `Loaded` 이벤트 추가
  - `IsPlayerLeft` 불리언 추가
- 각 토리 씬 상태의 편의를 개선하는 다음 항목들을 추가하였습니다.
  - `Interacted` 이벤트 추가
  - `IsForcedStayTimerTimedOut` 불리언 추가
  - `IsInteractionCheckTimerTimedOut` 불리언 추가
  - `IsTransitionTimertimedOut` 불리언 추가
  - `IsPlayerLeft` 불리언 추가
- 토리 인풋의 편의를 개선하는 다음 항목을 추가하였습니다.
  - `IsPlayerLeft` 불리언 추가

## Version 1.4.0

#### 피쳐 업데이트

- 에디터에서 `ToryValue`의 값을 클릭-앤-드래그로 변경할 수 있도록 하였습니다.
- `ToryValue`와 관련한 설명을 `README` 파일에 추가하였습니다.

## Version 1.3.0

#### 피쳐 업데이트

- `ScriptingDefineSymbolGenerator.cs` 스크립트가 심볼을 별도 관리합니다. (다른 분들은 신경쓰지 않아도 됩니다).
- `ToryScene`의 각 상태의 에디터를 보기 좋게 수정하였습니다.
- 개발 편의를 위해 `uREPL` 외부 라이브러리를 추가하였습니다.
- `ToryScene`과 각 상태 씬에 `PlayerLeft` 및 `TimedOut` 류의 이벤트를 추가하였습니다.
- `REAME` 파일을 갱신하였습니다. 

## Version 1.2.2

#### 버그 수정

- `ToryInput` 에디터의 버그가 수정되었습니다.
- `InteractionType`이 `Discontinuous`하고 입력이 없을 때, `RawValue`와 `ProcessedValue`가 리셋되지 않고 유지되던 문제가 해결되었습니다. `MultiInputs`도 함께 수정되었습니다.
- `OEFFrequency`의 `Value`가 바뀔 때, 필터의 파라미터가 정상적으로 업데이트되도록(= `UpdateParams()` 메소드가 불리도록) 수정되었습니다.
- `ToryScene`에 `Clear()` 메소드와 `Cleared` 이벤트가 추가되고, `IsCleared` 프로퍼티가 정상적으로 갱신됩니다.
- `ToryScene`의 `StageIndex`와 `StepIndex`가 1이 아니라 0부터 시작하도록 수정되었습니다.
- `TorySceneFlow (Example)` 애니메이터의 초기값이 잘못 설정되어 있던걸 고쳤습니다.

## Version 1.2.1

#### 버그 수정

- `InteractionType`으로 미처 변경되지 않은 코드를 마저 수정하였습니다.
- `ToryInputBehaviour`에서 `DataType`으로 `Key Code`를 선택했을 때, `Filter`와 `Gain` 관련 프로터티가 사라지도록 변경하였습니다.

#### 피처 업데이트

- 각종 설명이 수정 또는 추가되었습니다.

## Version 1.2.0

#### 피처 업데이트

- `Scripting Runtime Version`이 `Stabe(.NET 3.5 Equivalent)`에서 작동하도록 변경하였습니다.
- `ToryInput`의 `ContinuityType`의 명명이 `InteractionType`으로 변경되었습니다.

## Version 1.1.0

#### 리팩토링

- `ToryScene`이 구현되었습니다.
- `ToryInput`이 구현되었습니다.
- `ToryValue`가 구현되었습니다.
- `ToryTime`이 구현되었습니다.
- `ToryProgress`가 구현되었습니다.