import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { Dropdown, Button } from 'react-bootstrap';
import userImg from '../assets/image.png';

const Post = ({ post, onDelete, onEdit, onAddComment }) => {
  const [isEditing, setIsEditing] = useState(false);
  const [editedContent, setEditedContent] = useState(post?.content || "");
  const [comments, setComments] = useState(post?.comments || []);
  const [newComment, setNewComment] = useState("");

  // Handles deleting a post
  const handleDelete = () => {
    if (onDelete) {
      onDelete(post.id);
    }
  };

  // Enables the editing state
  const handleEdit = () => {
    setIsEditing(true);
  };

  // Saves the edited post
  const handleSave = () => {
    if (onEdit) {
      onEdit(post.id, editedContent);
      setIsEditing(false);
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
    if (!newComment.trim()) {
      return;
    }
    const comment = {
      id: Date.now(), // Temporary ID
      author: 'Current User', // Replace with actual logged-in user details
      content: newComment,
      createdAt: new Date().toLocaleString(),
    };
    setComments([comment, ...comments]);
    setNewComment("");
    if (onAddComment) {
      onAddComment(post.id, comment);
    }
  };

  // Format the date safely
  const formatDate = (dateString) => {
    if (!dateString) return "Unknown Date";
    const date = new Date(dateString);
    return isNaN(date) ? "Invalid Date" : date.toLocaleDateString(undefined, {
      year: "numeric",
      month: "short",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  // Safeguard to avoid rendering errors when post is undefined
  if (!post) {
    return null;
  }

  return (
    <div className="post">
      {/* Left Content - Image or Note Content (if no image) */}
      <div className="left-content">
        {post.image ? (
          <div className="imagecontent">
            <img
              src={URL.createObjectURL(post.image)}
              alt="Post Content"
            />
          </div>
        ) : (
          <div className="notecontentwithoutimage">
            <p>{post.content}</p>
          </div>
        )}
      </div>

      {/* Right Content - User Details, Post Content, and Comments */}
      <div className="right-content">
        {/* Post Header Section */}
        <div className="post-details-box">
          <div className="post-header">
            <div className="userinfo">
              <div className="userImg">
                <img src={userImg} alt="User profile" />
              </div>
              <div>
                <span className="username">{post.author || "Unknown Author"}</span>
                <span className="post-date"> - {formatDate(post.createdAt)}</span>
              </div>
            </div>
            {/* Dropdown Menu for Post Options */}
            <Dropdown className="ml-auto">
              <Dropdown.Toggle variant="light" id={`dropdown-${post.id}`} className="dropdown-toggle">
                <i className="fas fa-ellipsis-v"></i>
              </Dropdown.Toggle>
              <Dropdown.Menu>
                <Dropdown.Item onClick={handleEdit}>Edit</Dropdown.Item>
                <Dropdown.Item onClick={handleDelete}>Delete</Dropdown.Item>
              </Dropdown.Menu>
            </Dropdown>
          </div>
        </div>

        {/* Post Content Section - Only show here if there is an image */}
        {post.image && !isEditing && (
          <div className="notecontentbelowuserinfo">
            <p>{post.content}</p>
          </div>
        )}

        {/* Edit Mode */}
        {isEditing && (
          <textarea
            value={editedContent}
            onChange={(e) => setEditedContent(e.target.value)}
            className="form-control mb-3 noteInput"
          />
        )}

        {/* Edit Buttons - Save and Cancel */}
        {isEditing && (
          <div className="edit-actions d-flex justify-content-end">
            <Button variant="primary" onClick={handleSave} className="buttonstandard mr-2">
              Save
            </Button>
            <Button variant="secondary" onClick={handleCancel} className="buttonstandardCancel">
              Cancel
            </Button>
          </div>
        )}

        {/* Comments Section */}
        <div className="commentsection">
          <h5 className="mb-3">Comments</h5>
          <div className="comments-scrollable mb-3">
            {comments.length > 0 ? (
              comments.map((comment) => (
                <div key={comment.id} className="comment-content">
                  <div className="comment-details-box">
                    <div className="commentuserinfo">
                      <div className="commentuserImg">
                        <img
                          src={userImg}
                          alt="User"
                          width="25"
                          height="25"
                        />
                      </div>
                      <span className="commentusername">{comment.author}</span>
                    </div>
                    <span className="comment-date">{comment.createdAt}</span>
                  </div>
                  <p className="comment">{comment.content}</p>
                </div>
              ))
            ) : (
              <p>No comments yet. Be the first to comment!</p>
            )}
          </div>
          <form className="comment-form" onSubmit={handleAddComment}>
            <textarea
              className="form-control mb-2"
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
    </div>
  );
};

// PropTypes for validation
Post.propTypes = {
  post: PropTypes.object.isRequired,
  onDelete: PropTypes.func.isRequired,
  onEdit: PropTypes.func.isRequired,
  onAddComment: PropTypes.func,
};

export default Post;
