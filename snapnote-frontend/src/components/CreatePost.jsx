import React, { useState } from 'react';

const CreatePost = ({ onPostCreated }) => {
  const [content, setContent] = useState('');
  const [image, setImage] = useState(null);

  const handleSubmit = (e) => {
    e.preventDefault();
    // Create a new post object
    const newPost = {
      id: Date.now(), // Temporary ID
      content,
      image,
      author: 'Current User', // Replace with logged-in user details
      date: new Date().toLocaleString(),
    };
    // Callback to add the new post to the feed
    onPostCreated(newPost);
    // Clear inputs
    setContent('');
    setImage(null);
  };

  return (
    <div className="create-post-container">
      <form onSubmit={handleSubmit}>
        <div className="left-column">
          {image ? (
            <div className="image-placeholder">
              <img src={URL.createObjectURL(image)} alt="Post Preview" />
            </div>
          ) : (
            <label className="custom-file-upload">
              <input
                type="file"
                onChange={(e) => setImage(e.target.files[0])}
                accept="image/*"
              />
              Upload Image
            </label>
          )}
        </div>
        <div className="right-column">
          <textarea
            id="noteInput"
            value={content}
            onChange={(e) => setContent(e.target.value)}
            placeholder="What's on your mind?"
            required
          />
          <button type="submit" className="postbutton">
            Post
          </button>
        </div>
      </form>
    </div>
  );
};

export default CreatePost;
