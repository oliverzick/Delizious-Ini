﻿namespace Delizious.Ini.Test;

using System;
using System.Collections.Generic;
using System.Linq;

[TestClass]
public sealed class IniDocumentConfigurationSpec
{
    private static IniDocumentConfiguration Target => IniDocumentConfiguration.Default;

    public static IEnumerable<Setting> AllSettings =>
    [
        Setting.CaseSensitivity,
        Setting.NewlineString,
        Setting.SectionBeginningDelimiter,
        Setting.SectionEndDelimiter,
        Setting.SectionNameRegex,
        Setting.DuplicatePropertyBehavior,
        Setting.DuplicateSectionBehavior,
        Setting.InvalidLineBehavior,
        Setting.PropertyAssignmentSeparator,
        Setting.PropertyAssignmentSpacer,
        Setting.PropertyEnumerationMode,
        Setting.PropertyReadMode,
        Setting.PropertyWriteMode,
        Setting.PropertyDeletionMode,
        Setting.SectionDeletionMode,
        Setting.CommentString,
        Setting.CommentReadMode,
        Setting.CommentWriteMode
    ];

    [TestClass]
    public sealed class Defaults
    {
        [DataTestMethod]
        [DynamicData(nameof(Default_case_sensitivity_test_cases), DynamicDataSourceType.Method)]
        public void Default_case_sensitivity(IniDocumentConfiguration target, CaseSensitivity expected)
        {
            var actual = target.CaseSensitivity;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_case_sensitivity_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, CaseSensitivity.CaseInsensitive];
            yield return [IniDocumentConfiguration.Loose, CaseSensitivity.CaseInsensitive];
            yield return [IniDocumentConfiguration.Strict, CaseSensitivity.CaseInsensitive];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_newline_string_test_cases), DynamicDataSourceType.Method)]
        public void Default_newline_string(IniDocumentConfiguration target, NewlineString expected)
        {
            var actual = target.NewlineString;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_newline_string_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, NewlineString.Environment];
            yield return [IniDocumentConfiguration.Loose, NewlineString.Environment];
            yield return [IniDocumentConfiguration.Strict, NewlineString.Environment];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_section_beginning_delimiter_test_cases), DynamicDataSourceType.Method)]
        public void Default_section_beginning_delimiter(IniDocumentConfiguration target, SectionBeginningDelimiter expected)
        {
            var actual = target.SectionBeginningDelimiter;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_section_beginning_delimiter_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, SectionBeginningDelimiter.Default];
            yield return [IniDocumentConfiguration.Loose, SectionBeginningDelimiter.Default];
            yield return [IniDocumentConfiguration.Strict, SectionBeginningDelimiter.Default];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_section_end_delimiter_test_cases), DynamicDataSourceType.Method)]
        public void Default_section_end_delimiter(IniDocumentConfiguration target, SectionEndDelimiter expected)
        {
            var actual = target.SectionEndDelimiter;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_section_end_delimiter_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, SectionEndDelimiter.Default];
            yield return [IniDocumentConfiguration.Loose, SectionEndDelimiter.Default];
            yield return [IniDocumentConfiguration.Strict, SectionEndDelimiter.Default];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_section_name_regex_test_cases), DynamicDataSourceType.Method)]
        public void Default_section_name_regex(IniDocumentConfiguration target, SectionNameRegex expected)
        {
            var actual = target.SectionNameRegex;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_section_name_regex_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, SectionNameRegex.Default];
            yield return [IniDocumentConfiguration.Loose, SectionNameRegex.Default];
            yield return [IniDocumentConfiguration.Strict, SectionNameRegex.Default];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_duplicate_property_behavior_test_cases), DynamicDataSourceType.Method)]
        public void Default_duplicate_property_behavior(IniDocumentConfiguration target, DuplicatePropertyBehavior expected)
        {
            var actual = target.DuplicatePropertyBehavior;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_duplicate_property_behavior_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, DuplicatePropertyBehavior.Ignore];
            yield return [IniDocumentConfiguration.Loose, DuplicatePropertyBehavior.Ignore];
            yield return [IniDocumentConfiguration.Strict, DuplicatePropertyBehavior.Fail];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_duplicate_section_behavior_test_cases), DynamicDataSourceType.Method)]
        public void Default_duplicate_section_behavior(IniDocumentConfiguration target, DuplicateSectionBehavior expected)
        {
            var actual = target.DuplicateSectionBehavior;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_duplicate_section_behavior_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, DuplicateSectionBehavior.Merge];
            yield return [IniDocumentConfiguration.Loose, DuplicateSectionBehavior.Merge];
            yield return [IniDocumentConfiguration.Strict, DuplicateSectionBehavior.Fail];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_invalid_line_behavior_test_cases), DynamicDataSourceType.Method)]
        public void Default_invalid_line_behavior(IniDocumentConfiguration target, InvalidLineBehavior expected)
        {
            var actual = target.InvalidLineBehavior;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_invalid_line_behavior_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, InvalidLineBehavior.Ignore];
            yield return [IniDocumentConfiguration.Loose, InvalidLineBehavior.Ignore];
            yield return [IniDocumentConfiguration.Strict, InvalidLineBehavior.Fail];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_property_assignment_separator_test_cases), DynamicDataSourceType.Method)]
        public void Default_property_assignment_separator(IniDocumentConfiguration target, PropertyAssignmentSeparator expected)
        {
            var actual = target.PropertyAssignmentSeparator;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_property_assignment_separator_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, PropertyAssignmentSeparator.Default];
            yield return [IniDocumentConfiguration.Loose, PropertyAssignmentSeparator.Default];
            yield return [IniDocumentConfiguration.Strict, PropertyAssignmentSeparator.Default];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_property_assignment_spacer_test_cases), DynamicDataSourceType.Method)]
        public void Default_property_assignment_spacer(IniDocumentConfiguration target, PropertyAssignmentSpacer expected)
        {
            var actual = target.PropertyAssignmentSpacer;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_property_assignment_spacer_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, PropertyAssignmentSpacer.None];
            yield return [IniDocumentConfiguration.Loose, PropertyAssignmentSpacer.None];
            yield return [IniDocumentConfiguration.Strict, PropertyAssignmentSpacer.None];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_property_enumeration_mode_test_cases), DynamicDataSourceType.Method)]
        public void Default_property_enumeration_mode(IniDocumentConfiguration target, PropertyEnumerationMode expected)
        {
            var actual = target.PropertyEnumerationMode;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_property_enumeration_mode_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, PropertyEnumerationMode.Fallback];
            yield return [IniDocumentConfiguration.Loose, PropertyEnumerationMode.Fallback];
            yield return [IniDocumentConfiguration.Strict, PropertyEnumerationMode.Fail];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_property_read_mode_test_cases), DynamicDataSourceType.Method)]
        public void Default_property_read_mode(IniDocumentConfiguration target, PropertyReadMode expected)
        {
            var actual = target.PropertyReadMode;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_property_read_mode_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, PropertyReadMode.Fallback];
            yield return [IniDocumentConfiguration.Loose, PropertyReadMode.Fallback];
            yield return [IniDocumentConfiguration.Strict, PropertyReadMode.Fail];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_property_write_mode_test_cases), DynamicDataSourceType.Method)]
        public void Default_property_write_mode(IniDocumentConfiguration target, PropertyWriteMode expected)
        {
            var actual = target.PropertyWriteMode;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_property_write_mode_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, PropertyWriteMode.Create];
            yield return [IniDocumentConfiguration.Loose, PropertyWriteMode.Create];
            yield return [IniDocumentConfiguration.Strict, PropertyWriteMode.Update];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_property_deletion_mode_test_cases), DynamicDataSourceType.Method)]
        public void Default_property_deletion_mode(IniDocumentConfiguration target, PropertyDeletionMode expected)
        {
            var actual = target.PropertyDeletionMode;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_property_deletion_mode_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, PropertyDeletionMode.Ignore];
            yield return [IniDocumentConfiguration.Loose, PropertyDeletionMode.Ignore];
            yield return [IniDocumentConfiguration.Strict, PropertyDeletionMode.Fail];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_section_deletion_mode_test_cases), DynamicDataSourceType.Method)]
        public void Default_section_deletion_mode(IniDocumentConfiguration target, SectionDeletionMode expected)
        {
            var actual = target.SectionDeletionMode;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_section_deletion_mode_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, SectionDeletionMode.Ignore];
            yield return [IniDocumentConfiguration.Loose, SectionDeletionMode.Ignore];
            yield return [IniDocumentConfiguration.Strict, SectionDeletionMode.Fail];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_comment_string_test_cases), DynamicDataSourceType.Method)]
        public void Default_comment_string(IniDocumentConfiguration target, CommentString expected)
        {
            var actual = target.CommentString;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_comment_string_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, CommentString.Default];
            yield return [IniDocumentConfiguration.Loose, CommentString.Default];
            yield return [IniDocumentConfiguration.Strict, CommentString.Default];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_comment_read_mode_test_cases), DynamicDataSourceType.Method)]
        public void Default_comment_read_mode(IniDocumentConfiguration target, CommentReadMode expected)
        {
            var actual = target.CommentReadMode;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_comment_read_mode_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, CommentReadMode.Fallback];
            yield return [IniDocumentConfiguration.Loose, CommentReadMode.Fallback];
            yield return [IniDocumentConfiguration.Strict, CommentReadMode.Fail];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_comment_write_mode_test_cases), DynamicDataSourceType.Method)]
        public void Default_comment_write_mode(IniDocumentConfiguration target, CommentWriteMode expected)
        {
            var actual = target.CommentWriteMode;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_comment_write_mode_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, CommentWriteMode.Ignore];
            yield return [IniDocumentConfiguration.Loose, CommentWriteMode.Ignore];
            yield return [IniDocumentConfiguration.Strict, CommentWriteMode.Fail];
        }
    }

    [TestClass]
    public sealed class WithCaseSensitivity
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.CaseSensitivity];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_case_sensitivity_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithCaseSensitivity(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithCaseSensitivity(CaseSensitivity.CaseSensitive);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithNewlineString
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.NewlineString];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_newline_string_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithNewlineString(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            // ToDo: Use different newline string in regard to current environment?
            var actual = original.WithNewlineString(NewlineString.Unix);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithSectionBeginningDelimiter
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.SectionBeginningDelimiter];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_section_beginning_delimiter_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithSectionBeginningDelimiter(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithSectionBeginningDelimiter('(');

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithSectionEndDelimiter
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.SectionEndDelimiter];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_section_end_delimiter_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithSectionEndDelimiter(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithSectionEndDelimiter(')');

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithSectionNameRegex
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.SectionNameRegex];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_section_regex_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithSectionNameRegex(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithSectionNameRegex(SectionNameRegex.Create(@"\w+"));

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithDuplicatePropertyBehavior
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.DuplicatePropertyBehavior];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_duplicate_property_behavior_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithDuplicatePropertyBehavior(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithDuplicatePropertyBehavior(DuplicatePropertyBehavior.Fail);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithDuplicateSectionBehavior
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.DuplicateSectionBehavior];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_duplicate_section_behavior_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithDuplicateSectionBehavior(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithDuplicateSectionBehavior(DuplicateSectionBehavior.Fail);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithInvalidLineBehavior
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.InvalidLineBehavior];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_invalid_line_behavior_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithInvalidLineBehavior(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithInvalidLineBehavior(InvalidLineBehavior.Fail);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithPropertyAssignmentSeparator
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.PropertyAssignmentSeparator];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_property_assignment_separator_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyAssignmentSeparator(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithPropertyAssignmentSeparator(':');

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithPropertyAssignmentSpacer
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.PropertyAssignmentSpacer];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_property_assignment_spacer_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyAssignmentSpacer(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithPropertyAssignmentSpacer(PropertyAssignmentSpacer.Space);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithPropertyEnumerationMode
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.PropertyEnumerationMode];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_property_enumeration_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyEnumerationMode(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithPropertyEnumerationMode(PropertyEnumerationMode.Fail);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithPropertyReadMode
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.PropertyReadMode];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_property_read_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyReadMode(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithPropertyReadMode(PropertyReadMode.Fail);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithPropertyWriteMode
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.PropertyWriteMode];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_property_write_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyWriteMode(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithPropertyWriteMode(PropertyWriteMode.Update);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithPropertyDeletionMode
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.PropertyDeletionMode];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_property_deletion_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyDeletionMode(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithPropertyDeletionMode(PropertyDeletionMode.Fail);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithSectionDeletionMode
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.SectionDeletionMode];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_section_deletion_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithSectionDeletionMode(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithSectionDeletionMode(SectionDeletionMode.Fail);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithCommentString
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.CommentString];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_comment_string_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithCommentString(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithCommentString("/");

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithCommentReadMode
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.CommentReadMode];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_comment_read_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithCommentReadMode(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithCommentReadMode(CommentReadMode.Fail);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithCommentWriteMode
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.CommentWriteMode];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_comment_write_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithCommentWriteMode(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithCommentWriteMode(CommentWriteMode.Fail);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class ValueSemantics
    {
        private static IniDocumentConfiguration Null => null!;
        private static IniDocumentConfiguration Default => IniDocumentConfiguration.Default;
        private static IniDocumentConfiguration Loose => IniDocumentConfiguration.Loose;
        private static IniDocumentConfiguration Strict => IniDocumentConfiguration.Strict;

        [DataTestMethod]
        [DynamicData(nameof(Provides_string_representation_test_cases), DynamicDataSourceType.Method)]
        public void Provides_string_representation(IniDocumentConfiguration target)
        {
            var expected = $"{nameof(IniDocumentConfiguration)} {{ {string.Join(", ", AllSettings.Select(setting => setting.BuildString(target)))} }}";

            var actual = target.ToString();

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Provides_string_representation_test_cases()
        {
            yield return [Default];
            yield return [Loose];
            yield return [Strict];

            // ToDo: Reintroduce cases for single settings?
        }

        [DataTestMethod]
        [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
        public void Properly_implements_equality_operator(IniDocumentConfiguration left, IniDocumentConfiguration right, bool expected)
        {
            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
        public void Properly_implements_inequality_operator(IniDocumentConfiguration left, IniDocumentConfiguration right, bool unexpected)
        {
            var expected = !unexpected;

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equals_test_cases),      DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_equals_method(IniDocumentConfiguration target, object other, bool expected)
        {
            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_test_cases),         DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases),    DynamicDataSourceType.Method)]
        public void Properly_implements_general_equals_method(IniDocumentConfiguration target, object other, bool expected)
        {
            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_get_hash_code_method(IniDocumentConfiguration target, IniDocumentConfiguration other, bool expected)
        {
            var actual = target.GetHashCode() == other.GetHashCode();

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Equality_operator_test_cases()
        {
            yield return [Null, Null, true];
            yield return [Null, Default, false];
            yield return [Null, Loose, false];
            yield return [Null, Strict, false];
        }

        public static IEnumerable<object[]> Equals_null_test_cases()
        {
            yield return [Default, Null, false];
            yield return [Loose, Null, false];
            yield return [Strict, Null, false];
        }

        public static IEnumerable<object[]> Equals_test_cases()
        {
            yield return [Default, Default, true];
            yield return [Default, Loose, true];
            yield return [Default, Strict, false];
            yield return [Default, Default.WithCaseSensitivity(CaseSensitivity.CaseSensitive), false];
            yield return [Default, Default.WithCaseSensitivity(CaseSensitivity.CaseInsensitive), true];
            yield return [Default, Default.WithNewlineString(NewlineString.Environment), true];
            yield return [Default, Default.WithNewlineString(NewlineString.Unix), NewlineString.Unix == NewlineString.Environment];
            yield return [Default, Default.WithNewlineString(NewlineString.Windows), NewlineString.Windows == NewlineString.Environment];
            yield return [Default, Default.WithSectionBeginningDelimiter(SectionBeginningDelimiter.Default), true];
            yield return [Default, Default.WithSectionBeginningDelimiter('('), false];
            yield return [Default, Default.WithSectionEndDelimiter(SectionEndDelimiter.Default), true];
            yield return [Default, Default.WithSectionEndDelimiter(')'), false];
            yield return [Default, Default.WithDuplicatePropertyBehavior(DuplicatePropertyBehavior.Fail), false];
            yield return [Default, Default.WithDuplicatePropertyBehavior(DuplicatePropertyBehavior.Ignore), true];
            yield return [Default, Default.WithDuplicatePropertyBehavior(DuplicatePropertyBehavior.Override), false];
            yield return [Default, Default.WithDuplicateSectionBehavior(DuplicateSectionBehavior.Fail), false];
            yield return [Default, Default.WithDuplicateSectionBehavior(DuplicateSectionBehavior.Merge), true];
            yield return [Default, Default.WithPropertyDeletionMode(PropertyDeletionMode.Fail), false];
            yield return [Default, Default.WithPropertyDeletionMode(PropertyDeletionMode.Ignore), true];
            yield return [Default, Default.WithPropertyAssignmentSeparator(PropertyAssignmentSeparator.Default), true];
            yield return [Default, Default.WithPropertyAssignmentSeparator(':'), false];
            yield return [Default, Default.WithPropertyEnumerationMode(PropertyEnumerationMode.Fail), false];
            yield return [Default, Default.WithPropertyEnumerationMode(PropertyEnumerationMode.Fallback), true];
            yield return [Default, Default.WithPropertyReadMode(PropertyReadMode.Fail), false];
            yield return [Default, Default.WithPropertyReadMode(PropertyReadMode.Fallback), true];
            yield return [Default, Default.WithPropertyWriteMode(PropertyWriteMode.Create), true];
            yield return [Default, Default.WithPropertyWriteMode(PropertyWriteMode.Update), false];
            yield return [Default, Default.WithSectionDeletionMode(SectionDeletionMode.Fail), false];
            yield return [Default, Default.WithSectionDeletionMode(SectionDeletionMode.Ignore), true];
            yield return [Default, Default.WithCommentString(CommentString.Default), true];
            yield return [Default, Default.WithCommentString("/"), false];
            yield return [Default, Default.WithCommentReadMode(CommentReadMode.Fail), false];
            yield return [Default, Default.WithCommentReadMode(CommentReadMode.Fallback), true];
            yield return [Default, Default.WithCommentWriteMode(CommentWriteMode.Fail), false];
            yield return [Default, Default.WithCommentWriteMode(CommentWriteMode.Ignore), true];

            yield return [Loose, Default, true];
            yield return [Loose, Loose, true];
            yield return [Loose, Strict, false];

            yield return [Strict, Default, false];
            yield return [Strict, Loose, false];
            yield return [Strict, Strict, true];
        }

        public static IEnumerable<object[]> General_equals_test_cases()
        {
            yield return [Default, new(), false];
            yield return [Loose, new(), false];
            yield return [Strict, new(), false];
        }
    }

    private static object[] ToTestCase(Setting setting)
        => [setting];

    public sealed record Setting
    {
        private readonly Strategy strategy;

        private Setting(Strategy strategy)
        {
            this.strategy = strategy;
        }

        public void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
            => this.strategy.AssertIsEqual(original, actual);

        public string BuildString(IniDocumentConfiguration target)
            => this.strategy.BuildString(target);

        public override string ToString()
            => this.strategy.ToString();

        private abstract record Strategy
        {
            public abstract void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual);

            public abstract string BuildString(IniDocumentConfiguration target);
        }

        public static Setting CaseSensitivity
            => new(new CaseSensitivitySetting());

        private sealed record CaseSensitivitySetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.CaseSensitivity, actual.CaseSensitivity);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.CaseSensitivity)} = {target.CaseSensitivity}";
        }

        public static Setting NewlineString
            => new(new NewlineStringSetting());

        private sealed record NewlineStringSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.NewlineString, actual.NewlineString);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.NewlineString)} = {target.NewlineString}";
        }

        public static Setting SectionBeginningDelimiter
            => new(new SectionBeginningDelimiterSetting());

        private sealed record SectionBeginningDelimiterSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.SectionBeginningDelimiter, actual.SectionBeginningDelimiter);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.SectionBeginningDelimiter)} = {target.SectionBeginningDelimiter}";
        }

        public static Setting SectionEndDelimiter
            => new(new SectionEndDelimiterSetting());

        private sealed record SectionEndDelimiterSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.SectionEndDelimiter, actual.SectionEndDelimiter);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.SectionEndDelimiter)} = {target.SectionEndDelimiter}";
        }

        public static Setting SectionNameRegex
            => new(new SectionNameRegexSetting());

        private sealed record SectionNameRegexSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.SectionNameRegex, actual.SectionNameRegex);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.SectionNameRegex)} = {target.SectionNameRegex}";
        }

        public static Setting DuplicatePropertyBehavior
            => new(new DuplicatePropertyBehaviorSetting());

        private sealed record DuplicatePropertyBehaviorSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.DuplicatePropertyBehavior, actual.DuplicatePropertyBehavior);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.DuplicatePropertyBehavior)} = {target.DuplicatePropertyBehavior}";
        }

        public static Setting DuplicateSectionBehavior
            => new(new DuplicateSectionBehaviorSetting());

        private sealed record DuplicateSectionBehaviorSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.DuplicateSectionBehavior, actual.DuplicateSectionBehavior);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.DuplicateSectionBehavior)} = {target.DuplicateSectionBehavior}";
        }

        public static Setting InvalidLineBehavior
            => new(new InvalidLineBehaviorSetting());

        private sealed record InvalidLineBehaviorSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.InvalidLineBehavior, actual.InvalidLineBehavior);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.InvalidLineBehavior)} = {target.InvalidLineBehavior}";
        }

        public static Setting PropertyAssignmentSeparator
            => new(new PropertyAssignmentSeparatorSetting());

        private sealed record PropertyAssignmentSeparatorSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyAssignmentSeparator, actual.PropertyAssignmentSeparator);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.PropertyAssignmentSeparator)} = {target.PropertyAssignmentSeparator}";
        }

        public static Setting PropertyAssignmentSpacer
            => new(new PropertyAssignmentSpacerSetting());

        private sealed record PropertyAssignmentSpacerSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyAssignmentSpacer, actual.PropertyAssignmentSpacer);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.PropertyAssignmentSpacer)} = {target.PropertyAssignmentSpacer}";
        }

        public static Setting PropertyEnumerationMode
            => new(new PropertyEnumerationModeSetting());

        private sealed record PropertyEnumerationModeSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyEnumerationMode, actual.PropertyEnumerationMode);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.PropertyEnumerationMode)} = {target.PropertyEnumerationMode}";
        }

        public static Setting PropertyReadMode
            => new(new PropertyReadModeSetting());

        private sealed record PropertyReadModeSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyReadMode, actual.PropertyReadMode);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.PropertyReadMode)} = {target.PropertyReadMode}";
        }

        public static Setting PropertyWriteMode
            => new(new PropertyWriteModeSetting());

        private sealed record PropertyWriteModeSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyWriteMode, actual.PropertyWriteMode);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.PropertyWriteMode)} = {target.PropertyWriteMode}";
        }

        public static Setting PropertyDeletionMode
            => new(new PropertyDeletionModeSetting());

        private sealed record PropertyDeletionModeSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyDeletionMode, actual.PropertyDeletionMode);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.PropertyDeletionMode)} = {target.PropertyDeletionMode}";
        }

        public static Setting SectionDeletionMode
            => new(new SectionDeletionModeSetting());

        private sealed record SectionDeletionModeSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.SectionDeletionMode, actual.SectionDeletionMode);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.SectionDeletionMode)} = {target.SectionDeletionMode}";
        }

        public static Setting CommentString
            => new(new CommentStringSetting());

        private sealed record CommentStringSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.CommentString, actual.CommentString);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.CommentString)} = {target.CommentString}";
        }

        public static Setting CommentReadMode
            => new(new CommentReadModeSetting());

        private sealed record CommentReadModeSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.CommentReadMode, actual.CommentReadMode);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.CommentReadMode)} = {target.CommentReadMode}";
        }

        public static Setting CommentWriteMode
            => new(new CommentWriteModeSetting());

        private sealed record CommentWriteModeSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.CommentWriteMode, actual.CommentWriteMode);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.CommentWriteMode)} = {target.CommentWriteMode}";
        }
    }
}
