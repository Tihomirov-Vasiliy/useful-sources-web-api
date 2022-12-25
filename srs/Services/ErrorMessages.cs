namespace Services
{
    public static class ErrorMessages
    {
        //Sources error messages
        public const string GET_SOURCES_404_NOT_FOUNDED = "Sources not found in database";
        public const string SOURCE_NOT_FOUNDED = "Source not found by id: ";
        public const string CREATE_SOUCE_NULL_SOURCE_INPUT = "You can't create source (source is null)";
        public const string CREATE_SOURCE_WRONG_TAG_INPUT = "You can't create source in that way. You can add only existing tags to source. " +
            "Check the rigth way to do it in documentation for current API ";
        public const string UPDATE_SOURCE_WRONG_TAG_INPUT = "You can't replace/add tags to source because there is no tags with id: ";

        //Tags error messages
        public const string TAGS_NOT_FOUNDED = "Tags not found in database";
        public const string TAG_NOT_FOUNDED = "Tag not found by id: ";
        public const string CREATE_TAG_NULL_TAG_INPUT = "You cant' create tag (tag is null)";
        public const string CREATE_TAG_WRONG_INPUT = "You can't create tag in that way. You can add only existing tags to parent tags of new tag. " +
            "Check the rigth way to do it in documentation for current API ";
        public const string UPDATE_TAG_WRONG_TAG_INPUT = "You can't replace/add parent tags because there is no tags with id: ";
        public const string UPDATE_TAG_SAME_TAG_INPUT = "Tag can't reffer as parent tag to itself";
        public const string DELETE_TAG_WRONG_INPUT = "You can't delete tag in that way. You can delete only tags without child tags. " +
            "Check the rigth way to do it in documentation for current API. Or delete tags at ids: ";
        
        //Authentication error messages
        public const string USER_NOT_FOUND = "User with combination of login and password not found";

        //Database error messages
        public const string SOMETHING_WENT_WRONG_IN_DATABASE = "Something went wrong in server";
    }
}
