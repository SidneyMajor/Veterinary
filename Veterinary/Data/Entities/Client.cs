namespace Veterinary.Data.Entities
{
    public class Client : Person
    {
        public string FullName { get { return $"{FirstName} {LastName}"; } }

        public string ImagePath
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ImageUrl))
                {
                    return this.ImageUrl.Replace("~", "..");
                }
                else
                {
                    return "../images/img.jpg";
                }


            }
        }
    }
}
