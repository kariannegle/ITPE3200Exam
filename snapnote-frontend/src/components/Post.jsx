// src/components/Post.jsx
import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { Dropdown, Button } from 'react-bootstrap';

const Post = ({ post, onDelete, onEdit, onAddComment }) => {
  const [isEditing, setIsEditing] = useState(false);
  const [editedContent, setEditedContent] = useState(post.content);
  const [comments, setComments] = useState(post.comments || []); // Holds current comments
  const [newComment, setNewComment] = useState("");

  // Handles deleting a post
  const handleDelete = () => {
    if (typeof onDelete === 'function') {
      onDelete(post.id);
    } else {
      console.error("onDelete is not a function in Post!");
    }
  };

  // Enables the editing state
  const handleEdit = () => {
    setIsEditing(true);
  };

  // Saves the edited post
  const handleSave = () => {
    if (typeof onEdit === 'function') {
      onEdit(post.id, editedContent);
      setIsEditing(false);
    } else {
      console.error("onEdit is not a function in Post!");
    }
  };

  // Cancels editing
  const handleCancel = () => {
    setEditedContent(post.content);
    setIsEditing(false);
  };

  // Handles adding a new comment
  const handleAddComment = (e) => {
    e.preventDefault();

    if (newComment.trim() === "") {
      return;
    }

    const comment = {
      id: Date.now(), // Temporary ID
      author: 'Current User', // Replace with logged-in user details
      content: newComment,
      createdAt: new Date().toLocaleString()
    };

    setComments([comment, ...comments]);
    setNewComment("");

    if (typeof onAddComment === 'function') {
      onAddComment(post.id, comment);
    }
  };

  return (
    <div className="post">
      <div className="post-details-box">
        {/* User Information Section */}
        <div className="userinfo">
          <div className="userImg">
            <img src="default-user.png" alt="User" />
          </div>
          <span className="username">{post.author}</span>
        </div>

        {/* Post Header Section */}
        <div className="post-header">
          {isEditing ? (
            <textarea
              value={editedContent}
              onChange={(e) => setEditedContent(e.target.value)}
              className="edit-textarea"
            />
          ) : (
            <p className="notecontentwithoutimage">{post.content}</p>
          )}

          {/* Dropdown Menu for Post Options */}
          <Dropdown className="dropdown-container">
            <Dropdown.Toggle variant="secondary" id={`dropdown-${post.id}`} className="dropdown-toggle">
              <i className="fas fa-ellipsis-v"></i>
            </Dropdown.Toggle>

            <Dropdown.Menu>
              <Dropdown.Item onClick={handleEdit}>Edit</Dropdown.Item>
              <Dropdown.Item onClick={handleDelete}>Delete</Dropdown.Item>
            </Dropdown.Menu>
          </Dropdown>

          {/* Edit Buttons - Save and Cancel */}
          {isEditing && (
            <div className="edit-actions">
              <Button variant="primary" onClick={handleSave} className="buttonstandard">
                Save
              </Button>
              <Button variant="secondary" onClick={handleCancel} className="buttonstandardCancel">
                Cancel
              </Button>
            </div>
          )}
        </div>
      </div>

      {/* Post Image Section */}
      {post.image && (
        <div className="imagecontent">
          <img src={URL.createObjectURL(post.image)} alt="Post Content" />
        </div>
      )}

      {/* Comment Section */}
      <div className="commentsection">
        <h4>Comments</h4>
        <div className="comments-scrollable">
          {comments.map((comment) => (
            <div key={comment.id} className="comment-content">
              <div className="comment-details-box">
                <div className="commentuserinfo">
                  <div className="commentuserImg">
                    <img src="default-user.png" alt="User" />
                  </div>
                  <span className="commentusername">{comment.author}</span>
                </div>
                <span className="comment-date">{comment.createdAt}</span>
              </div>
              <p className="comment">{comment.content}</p>
            </div>
          ))}
        </div>
        <form className="comment-form" onSubmit={handleAddComment}>
          <textarea
            className="form-control"
            value={newComment}
            onChange={(e) => setNewComment(e.target.value)}
            placeholder="Add a comment..."
            required
          />
          <button type="submit" className="submitcomment">
            Add Comment
          </button>
        </form>
      </div>
    </div>
  );
};

// PropTypes for validation
Post.propTypes = {
  post: PropTypes.object.isRequired,
  onDelete: PropTypes.func.isRequired,
  onEdit: PropTypes.func.isRequired,
  onAddComment: PropTypes.func // Optional callback for adding comments
};

export default Post;
