namespace Orc.FilterBuilder;

using Catel.ComponentModel;

public enum Condition
{
    [DisplayName("FilterBuilder_Contains")]
    Contains,
    [DisplayName("FilterBuilder_StartsWith")]
    StartsWith,
    [DisplayName("FilterBuilder_EndsWith")]
    EndsWith,
    [DisplayName("FilterBuilder_EqualTo")]
    EqualTo,
    [DisplayName("FilterBuilder_NotEqualTo")]
    NotEqualTo,
    [DisplayName("FilterBuilder_GreaterThan")]
    GreaterThan,
    [DisplayName("FilterBuilder_LessThan")]
    LessThan,
    [DisplayName("FilterBuilder_GreaterThanOrEqualTo")]
    GreaterThanOrEqualTo,
    [DisplayName("FilterBuilder_LessThanOrEqualTo")]
    LessThanOrEqualTo,
    [DisplayName("FilterBuilder_IsEmpty")]
    IsEmpty,
    [DisplayName("FilterBuilder_NotIsEmpty")]
    NotIsEmpty,
    [DisplayName("FilterBuilder_IsNull")]
    IsNull,
    [DisplayName("FilterBuilder_NotIsNull")]
    NotIsNull,
    [DisplayName("FilterBuilder_Matches")]
    Matches,
    [DisplayName("FilterBuilder_DoesNotMatch")]
    DoesNotMatch,
    [DisplayName("FilterBuilder_DoesNotContain")]
    DoesNotContain,
    [DisplayName("FilterBuilder_DoesNotStartWith")]
    DoesNotStartWith,
    [DisplayName("FilterBuilder_DoesNotEndWith")]
    DoesNotEndWith
}