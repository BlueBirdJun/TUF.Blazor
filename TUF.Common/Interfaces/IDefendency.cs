using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daniel.Common.Interfaces;

/// <summary>
/// 컴포넌트에 접근될 때마다 인스턴스가 생성됩니다.
/// </summary>
public interface ITransient
{
}
/// <summary>
/// 이용 고객 단위로 인스턴스가 생성됩니다.각 컴포넌트 간에 공통의 인스턴스가 이용됩니다.
/// </summary>
public interface IScoped
{
}
/// <summary>
/// 애플리케이션 전체에서 인스턴스가 생성됩니다.그래서 다른 유저 사이에서도 인스턴스가 공유됩니다.
/// </summary>
public interface ISingleton
{
}
public interface INotificationMessage
{
}