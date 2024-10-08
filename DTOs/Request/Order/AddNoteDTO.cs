/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the AddNoteDTO, which is used to capture a note to be added
 * to an order in the e-commerce system.
 * Date Created: 2024/09/28
 */

using System;

namespace ECommerceBackend.DTOs.Request.Order
{
    /// <summary>
    /// Represents a request to add a note to an order.
    /// </summary>
    public class AddNoteDTO
    {
        /// <summary>
        /// Gets or sets the content of the note.
        /// </summary>
        public string Note { get; set; } = string.Empty;
    }
}
