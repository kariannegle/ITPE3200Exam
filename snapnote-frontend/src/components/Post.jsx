// src/components/Post.jsx
import React from 'react';
import PropTypes from 'prop-types';
import { Dropdown } from 'react-bootstrap';

const Post = ({ post, onDelete }) => {
  console.log("Post received onDelete:", typeof onDelete);

  const handleDelete = () => {
    if (typeof onDelete === 'function') {
      // Optional: You can also handle confirmation here if not done in Home.jsx
      onDelete(post.id);
    } else {
      console.error("onDelete is not a function in Post!");
    }
  };

  return (
    <div className="post">
      <div className="post-details-box">
        <div className="userinfo">
          <div className="userImg">
            <img src="default-user.png" alt="User" />
          </div>
          <span className="username">{post.author}</span>
        </div>
        <div className="post-header">
          <span className="post-date">{post.date}</span>
          <div className="dropdown-container">
            <Dropdown>
              <Dropdown.Toggle
                variant="secondary"
                className="dropdown-toggle"
                id={`dropdown-${post.id}`}
              >
                <i className="fas fa-ellipsis-v"></i>
              </Dropdown.Toggle>

              <Dropdown.Menu>
                <Dropdown.Item onClick={handleDelete}>Delete</Dropdown.Item>
                <Dropdown.Item onClick={() => alert('Edit feature not yet implemented')}>
                  Edit
                </Dropdown.Item>
              </Dropdown.Menu>
            </Dropdown>
          </div>
        </div>
      </div>

      {post.image && (
        <div className="imagecontent">
          <img
            src={
              typeof post.image === 'string'
                ? post.image
                : URL.createObjectURL(post.image)
            }
            alt="Post Content"
          />
        </div>
      )}

      <div className="notecontentwithoutimage">
        <p>{post.content}</p>
      </div>
    </div>
  );
};

Post.propTypes = {
  post: PropTypes.shape({
    id: PropTypes.number.isRequired, // Assuming id is a number
    author: PropTypes.string.isRequired,
    date: PropTypes.string.isRequired,
    content: PropTypes.string.isRequired,
    image: PropTypes.oneOfType([
      PropTypes.instanceOf(File),
      PropTypes.string, // If image is a URL string
      PropTypes.oneOf([null]),
    ]),
  }).isRequired,
  onDelete: PropTypes.func.isRequired,
};

export default Post;
