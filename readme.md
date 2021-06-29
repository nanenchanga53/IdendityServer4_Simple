# IdentityServer4 를 이용한 OAuth 2.0 인증 예제
작성자 : 김우창

메일 : nanenchanga2@gmail.com

[<span style="color:rgb(255, 100, 100)">참조사이트</span>] https://identityserver4.readthedocs.io/en/latest/index.html
## 기본설정과 실행을 위한 속성
1. Server는 Core 3.0 을 이용해 제작하였고 Api 부분은 미들웨어 등록시 사용하는 Nuget Package가 각각 다르다
2. 이번 예제는 AOuth 2.0 의 OpenID 방식의 JWT를 이용한 예제다
3. 예제 실행시 솔루션 속성에서 시작 프로젝트를 여러 시작프로젝트를 선택한 후 서버와 API프로젝트를 선택하여 함께 실행한다.

![실행선택이미지](./image/StartProject.jpg)
4. 현재 상태로 API를 실행해보면 인증때문에 접속할 수 없다는 문구가 나올 것이다.

## PostMan 설정
1. PostMan에서 URL바로 아래에 <span style="color:rgb(255, 0, 0)">Authorization</span>탭을 선택
2. 좌측에서 Type 은 <span style="color:rgb(255, 0, 0)">OAuth 2.0</span> 선택하고 Add authorization data to 는 <span style="color:rgb(255, 0, 0)">Request Headers</span> 선택

    ![PostManStep1](./image/PostManStep1.jpg)
3. 우측에서 아래 그림을 참고하여 서버에서 설정한 값들을 넣는다 이번에는 Grant Type을 Client Credentails 을 사용하였다

    ![PostManStep2](./image/PostManStep2.jpg)
4.  아래의 <span style="color:rgb(255, 0, 0)">Get New Access Token</span>을 클릭해 실행되고 있는 서버에 연결해서 토큰을 받아오는지 확인한다. 결과가 아래 이미지와 비슷하게 나오면 성공이다. 

    ![PostManStep3](./image/PostManStep3.jpg)

5. <span style="color:rgb(255, 0, 0)">Use Token</span> 버튼을 클릭한후 Api에 연결하면 이전엔 인증때문에 접속할 수 없던 것이 이번엔 접속할 수 있을 것이다. 

<hr>
## 추가설명 

1. JWT를 사용하는 이유는 FrameWork 4 버전 이하에서 사용하는 API 인증에서 현제 사용되고 있지 않는 인증 방식을 피하기 위함
2. ID 등록을 포함하는 것은 B2B에 사용할 필요는 없을 것이라 판단하여 OpenID 방식 예제를 작성, B2C에 필요하다면 ID 등록예제를 추가할 예정
3. 소스에 사용자 정보를 집어 넣는 방식은 보안상 좋지 않은 방식이라고 한다 그러니 외부 DB를 생성해 사용자 정보를 저장해두고 불러오는 식으로 제작되어야 하는 것으로 보인다.
4. 내부에서 실행시 FrameWork Api 쪽 URL을 https로 하면 접속이 안될텐데 빈 프로젝트에서 생성한 IIS서버이기 때문이다. 이건 http쪽 Url을 사용하여 접속하자