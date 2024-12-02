import React, { useState } from 'react';

const CreatePost = ({ onPostCreated }) => {
  const [content, setContent] = useState('');
  const [image, setImage] = useState(null);

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setImage(file);
      const reader = new FileReader();
      reader.onload = (event) => {
        document.getElementById('imagePreview').src = event.target.result;
      };
      reader.readAsDataURL(file);
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const newPost = {
      id: Date.now(),
      content,
      image,
      author: 'Current User',
      date: new Date().toLocaleString(),
    };
    onPostCreated(newPost);
    setContent('');
    setImage(null);
    document.getElementById('imagePreview').src = '';
  };

  return (
    <div className="create-post-container">
      <form onSubmit={handleSubmit} className="create-post-form">
        {/* Left Column: Image Upload and Preview */}
        <div className="left-column">
          <div className="image-placeholder">
            <img
              id="imagePreview"
              alt="Selected"
              style={{ display: image ? 'block' : 'none' }}
            />
          </div>
          <label htmlFor="fileInput" className="custom-file-upload">
            <input
              type="file"
              id="fileInput"
              onChange={handleImageChange}
              accept="image/*"
              className="file-input"
              style={{ display: 'none' }}
            />
            Upload Image
          </label>
        </div>

        {/* Right Column: Content Input and Post Button */}
        <div className="right-column">
          <textarea
            id="noteInput"
            value={content}
            onChange={(e) => setContent(e.target.value)}
            placeholder="What's on your mind?"
            className="noteInput"
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
